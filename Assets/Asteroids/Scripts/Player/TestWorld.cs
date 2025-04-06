using Cysharp.Threading.Tasks;
using UnityEngine;

public class TestWorld : MonoBehaviour
{
    [SerializeField] private Vector3 worldSize = new Vector3(1000f, 1000f, 1000f);
    
    [SerializeField] private Transform _player;
    [SerializeField] private GameObject ufoPrefab;
    [SerializeField] private GameObject asteroidPrefab;

    [SerializeField] private float ufoSpawnRate = 5f;
    [SerializeField] private float asteroidSpawnRate = 10f;

    private void Start()
    {
        SpawnEnemies(ufoPrefab, ufoSpawnRate).Forget();
        SpawnEnemies(asteroidPrefab, asteroidSpawnRate).Forget();
    }
    
    private void Update()
    {
        WrapPosition(_player);
    }

    private void WrapPosition(Transform playerTransform)
    {
        var pos = playerTransform.position;
        if (pos.x > worldSize.x / 2) pos.x = (worldSize.x / 2) * -1;
        else if (pos.x < -worldSize.x / 2) pos.x = worldSize.x / 2;
        
        if (pos.y > worldSize.y / 2) pos.y = (worldSize.y / 2) * -1;
        else if (pos.y < -worldSize.y / 2) pos.y = worldSize.y / 2;
        
        if (pos.z > worldSize.z / 2) pos.z = (worldSize.z / 2) * -1;
        else if (pos.z < -worldSize.z / 2) pos.z = worldSize.z / 2;
        
        playerTransform.position = pos;
    }

    async UniTaskVoid SpawnEnemies(GameObject enemyPrefab, float spawnRate)
    {
        while (true)
        {
            var spawnPosition = GetRandomSpawnPosition();
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            await UniTask.Delay((int)(spawnRate * 1000));
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        var x = Random.Range(-worldSize.x * 1.1f, worldSize.x * 1.1f);
        var y = Random.Range(-worldSize.y * 1.1f, worldSize.y * 1.1f);
        var z = Random.Range(-worldSize.z * 1.1f, worldSize.z * 1.1f);

        if (Mathf.Abs(x) < worldSize.x && Mathf.Abs(y) < worldSize.y && Mathf.Abs(z) < worldSize.z)
        {
            return GetRandomSpawnPosition();
        }
        
        return new Vector3(x, y, z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, worldSize);
    }
}
