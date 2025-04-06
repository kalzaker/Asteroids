using Cysharp.Threading.Tasks;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    private int _hp = 3;
    private bool _isInvulnerable = false;
    [SerializeField] private float invulnerabilityTime = 3f;
    [SerializeField] private float knockbackForce = 5f;
    
    private Collider _playerCollider;
    private TestMove _playerMovement;

    private void Start()
    {
        _playerCollider = GetComponent<Collider>();
        _playerMovement = GetComponent<TestMove>();
    }

    public void TakeDamage()
    {
        if (_isInvulnerable) return;

        _hp--;
        if (_hp <= 0)
        {
            Time.timeScale = 0;
            Debug.Log("You Lose");
        }
        else
        {
            BecomeInvulnerable().Forget();
        }
    }

    public void Knockback(Vector3 direction)
    {
        var newPosition = transform.position + direction * knockbackForce;
        transform.Translate(newPosition, Space.World);
    }

    private async UniTaskVoid BecomeInvulnerable()
    {
        _isInvulnerable = true;
        _playerCollider.enabled = false;
        _playerMovement.enabled = false;
        
        await UniTask.Delay((int)(invulnerabilityTime * 1000));

        _isInvulnerable = false;
        _playerCollider.enabled = true;
        _playerMovement.enabled = true;
    }
}
