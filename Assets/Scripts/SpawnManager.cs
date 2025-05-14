using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _powerupPrefab;

    private int _waveNumber = 1;
    private float _spawnRange = 9;
    
    void Start()
    {
        Instantiate(_powerupPrefab, GenerateSpawnPosition(), _powerupPrefab.transform.rotation);
        SpawnWave(_waveNumber);
    }

    private void Update()
    {
        var enemyCount = FindObjectsOfType<EnemyController>().Length;
        
        if(enemyCount != 0) return;
        
        Instantiate(_powerupPrefab, GenerateSpawnPosition(), _powerupPrefab.transform.rotation);
        
        _waveNumber++;
        SpawnWave(_waveNumber);
            
    }

    private Vector3 GenerateSpawnPosition()
    {
        var spawnPositionX = Random.Range(-_spawnRange, _spawnRange);
        var spawnPositionZ = Random.Range(-_spawnRange, _spawnRange);
        var randomPosition = new Vector3(spawnPositionX, 0, spawnPositionZ);

        return randomPosition;
    }

    private void SpawnWave(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(_enemyPrefab, GenerateSpawnPosition(), _enemyPrefab.transform.rotation);    
        }   
    }
}
