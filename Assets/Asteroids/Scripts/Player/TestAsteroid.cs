using UnityEngine;
using Random = UnityEngine.Random;

public class TestAsteroid : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Vector3 _direction;
    
    [SerializeField] private GameObject smallAsteroidPrefab;
    [SerializeField] private int splitAmount = 4;

    [SerializeField] private bool isBigAsteroid;
    
    void Start()
    {
        _direction = Random.onUnitSphere;
        Invoke($"TakeDamage", 30f);
    }
    
    void Update()
    {
        transform.position += _direction * speed * Time.deltaTime;
    }

    public void TakeDamage()
    {
        Split();
        Destroy(gameObject);
    }

    private void Split()
    {
        if (!isBigAsteroid) return;
        
        for (int i = 0; i < splitAmount; i++)
        {
            Instantiate(smallAsteroidPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<TestPlayer>().TakeDamage();
            
            Destroy(gameObject);
        }
    }
}
