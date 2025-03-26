using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TestShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.2f;

    [SerializeField] private float laserRange = 50f;
    [SerializeField] private int maxLaserShots = 3;
    [SerializeField] private float laserCooldown = 7f;
    
    private int _currentLaserShotsAmount;
    private bool _isRecharging = false;
    private float _nextFireTime = 0f;

    private CancellationTokenSource _cts;

    private void Start()
    {
        _currentLaserShotsAmount = maxLaserShots;
        _cts = new CancellationTokenSource();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= _nextFireTime)
        {
            ShootBullet();
            _nextFireTime = Time.time + 1f / fireRate;
        }

        if (Input.GetMouseButtonDown(1) && _currentLaserShotsAmount > 0 && !_isRecharging)
        {
            ShootLaser();
            _currentLaserShotsAmount--;
            if (_currentLaserShotsAmount <= 0)
            {
                RechargeLaser(_cts.Token).Forget();
            }
        }
    }

    private void ShootBullet()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    private void ShootLaser()
    {
        RaycastHit hit;
        if (!Physics.Raycast(firePoint.position, firePoint.forward, out hit, laserRange)) return;
        Debug.DrawLine(firePoint.position, hit.point, Color.red, 0.5f);

        if (hit.collider.CompareTag($"Enemy"))
        {
            Destroy(hit.collider.gameObject);
        }
    }

    private async UniTaskVoid RechargeLaser(CancellationToken token)
    {
        _isRecharging = true;
        await UniTask.Delay((int) (laserCooldown * 1000), cancellationToken: token);
        _currentLaserShotsAmount = maxLaserShots;
        _isRecharging = false;
    }
}
