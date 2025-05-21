using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentWaveText;

    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private GameObject[] _powerupPrefabs;

    [SerializeField] private GameObject _bossEnemy;

    private int _waveNumber = 1;
    private float _spawnRange = 9;
    
    void Start()
    {
        SpawnWave(_waveNumber);
    }

    private void Update()
    {
        var enemyCount = FindObjectsOfType<EnemyController>().Length;
        
        if(enemyCount != 0) return;
        
        _waveNumber++;
        _currentWaveText.text = $"Wave: {_waveNumber}";
        SpawnWave(_waveNumber);
            
    }

    private Vector3 GenerateSpawnPosition(float yPosition)
    {
        var spawnPositionX = Random.Range(-_spawnRange, _spawnRange);
        var spawnPositionZ = Random.Range(-_spawnRange, _spawnRange);
        var randomPosition = new Vector3(spawnPositionX, yPosition, spawnPositionZ);

        return randomPosition;
    }

    private void SpawnWave(int amount)
    {
        if (amount % 5 == 0 && amount != 0)
        {
            Instantiate(_bossEnemy, GenerateSpawnPosition(3f), _bossEnemy.transform.rotation);
        }
        else
        {
            for (int i = 0; i < amount; i++)
            {
                var randomEnemy = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)];
                
                Instantiate(randomEnemy, GenerateSpawnPosition(0f), randomEnemy.transform.rotation);    
            }
        }
        
        var randomPowerup = _powerupPrefabs[Random.Range(0, _powerupPrefabs.Length)];
        
        Instantiate(randomPowerup, GenerateSpawnPosition(0f), randomPowerup.transform.rotation);
    }
}
