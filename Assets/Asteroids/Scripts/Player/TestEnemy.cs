using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    private GameObject _player;
    [SerializeField] private float speed = 3f;
    [SerializeField] private int hp = 2;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<TestPlayer>().TakeDamage();
            Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
            other.gameObject.GetComponent<TestPlayer>().Knockback(knockbackDirection);
            
            Knockback(-knockbackDirection);
        }
    }

    private void Update()
    {
        Vector3 direction = (_player.transform.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void TakeDamage()
    {
        hp--;
        
        if(hp <= 0) Destroy(gameObject);
    }

    private void Knockback(Vector3 direction)
    {
        Vector3 newPosition = transform.position + direction * 5f;
        transform.Translate(newPosition, Space.World);
    }
}