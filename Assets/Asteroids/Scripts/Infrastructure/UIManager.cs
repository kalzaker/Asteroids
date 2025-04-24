using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Zenject;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _laserChargesText;
    [SerializeField] private Slider _laserRechargeSlider;
    [SerializeField] private TextMeshProUGUI _positionText;
    [SerializeField] private TextMeshProUGUI _rotationText;
    [SerializeField] private TextMeshProUGUI _velocityText;
    
    private ShipModel _shipModel;
    private PlayerConfig _playerConfig;
    private int _score;

    private readonly Dictionary<(EnemyType, bool), int> scoreRewards = new Dictionary<(EnemyType, bool), int>
    {
        {(EnemyType.Asteroid, false), 5},
        {(EnemyType.Asteroid, true), 2},
        {(EnemyType.Ufo, false), 10}
    };

    [Inject]
    public void Construct(ShipModel shipModel)
    {
        _shipModel = shipModel;
        _playerConfig = ConfigLoader.LoadPlayerConfig();

        _shipModel.OnHealthChanged += UpdateHealthUI;
        _shipModel.OnLaserChargesChanged += UpdateLaserChargesUI;
        
        UpdateHealthUI(_shipModel.Health);
        UpdateLaserChargesUI(_shipModel.LaserCharges);
        _laserRechargeSlider.maxValue = _playerConfig.laserRechargeTime;
        _laserRechargeSlider.value = 0f;

        _score = 0;
        UpdateScoreUI();
    }

    private void OnDestroy()
    {
        _shipModel.OnHealthChanged -= UpdateHealthUI;
        _shipModel.OnLaserChargesChanged -= UpdateLaserChargesUI;
    }

    private void Update()
    {
        if (_shipModel.LaserCharges < _playerConfig.laserCharges)
        {
            float remainingTime = Mathf.Max(0, _playerConfig.laserRechargeTime - (Time.time - _shipModel.LastLaserUseTime));
            _laserRechargeSlider.value = remainingTime;
        }
        else
        {
            _laserRechargeSlider.value = 0f;
        }

        _positionText.text = $"Pos: ({_shipModel.Position.x:F1}, {_shipModel.Position.y:F1}, {_shipModel.Position.z:F1})";
        _rotationText.text = $"Rot: {_shipModel.Rotation.eulerAngles.z:F1}\u00b0";
        _velocityText.text = $"Vel: {_shipModel.Velocity.magnitude:F1}";
    }

    private void AddScoreForEnemy(IEnemy enemy)
    {
        bool isFragment = enemy.Type == EnemyType.Asteroid && ((AsteroidModel)enemy).IsFragment;
        if (scoreRewards.TryGetValue((enemy.Type, isFragment), out int points))
        {
            _score += points;
            UpdateScoreUI();
            
            int highScore = PlayerPrefs.GetInt("HighScore", 0);
            if (_score > highScore)
            {
                PlayerPrefs.SetInt("HighScore", _score);
                PlayerPrefs.Save();
            }
        }
    }

    public int GetScore()
    {
        return _score;
    }
    
    private void UpdateHealthUI(int health)
    {
        _healthText.text = $"Health: {health}";
    }

    private void UpdateLaserChargesUI(int charges)
    {
        _laserChargesText.text = $"Laser Charges: {charges}";
    }

    private void UpdateScoreUI()
    {
        _scoreText.text = $"Score: {_score}";
    }
}
