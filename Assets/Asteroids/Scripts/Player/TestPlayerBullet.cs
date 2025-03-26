using UnityEngine;

public class TestPlayerBullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float bulletLifeTime = 7f;

    void Start()
    {
        Invoke($"DestroyBullet", bulletLifeTime);
    }
    
    void Update()
    {
        transform.position += transform.forward * (speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag($"Enemy"))
        {
            other.gameObject.GetComponent<TestEnemy>().TakeDamage();
        }
        
        if (other.gameObject.CompareTag($"Asteroid"))
        {
            other.gameObject.GetComponent<TestAsteroid>().TakeDamage();
        }
        
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
