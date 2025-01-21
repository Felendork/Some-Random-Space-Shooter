using System.Collections;
using UnityEngine;
using UnityEngine.Lumin;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _enemies;
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _backgroundEnemy;
    [SerializeField]
    private GameObject _sideToSideEnemy;
    [SerializeField]
    private GameObject _stationaryEnemy;
    [SerializeField]
    private GameObject _enemyDodger;
    [SerializeField]
    private GameObject _boss;
    [SerializeField]
    private GameObject _astroid;

    [SerializeField]
    private GameObject[] _powerUps;

    [SerializeField]
    private GameObject[] _collectables;
    private GameObject _ammoCollectable;
    private GameObject _healthCollectable;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private bool _stopSpawning;

    [SerializeField]
    private int _currentLevel = 0;
    [SerializeField]
    private int _enemiesToSpawn;
    [SerializeField]
    private int _enemiesRemaining;

    private bool _isLevelEnding = true;

   private UIManager _manager;

    private void Start()
    {
        _manager = GameObject.Find("Canvas").GetComponent<UIManager>();

        _enemyPrefab = _enemies[0];
        _sideToSideEnemy = _enemies[1];
        _stationaryEnemy = _enemies[2];

        _ammoCollectable = _collectables[0];
        _healthCollectable = _collectables[1];
    }

    private void Update()
    {
        if (_enemiesRemaining == 0 && _isLevelEnding == true)
        {
            NewLevel();
            StopAllCoroutines();
        }
    }
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());

        StartCoroutine(SpawnBackgroundEnemyRoutine());

        StartCoroutine(SpawnPowerupRoutine());

        StartCoroutine(SpawnCommonCollectableRoutine());

        StartCoroutine(SpawnRareCollectableRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        _currentLevel++;
        _enemiesToSpawn = 5 + (_currentLevel * 5);
        _enemiesRemaining = _enemiesToSpawn;

        _manager.WaveTextUpdate(_currentLevel);

        while (_stopSpawning == false && _enemiesToSpawn > 0)
        {
            if (_currentLevel == 1)
            {
                yield return new WaitForSeconds(3f);
                Vector3 spawnPos = new Vector3(Random.Range(9.2f, -9.2f), 8.17f, 0);
                Quaternion rotPos = Quaternion.Euler(0, 0, 180);

                GameObject newEnemy = Instantiate(_enemyPrefab, spawnPos, rotPos);
                newEnemy.transform.parent = _enemyContainer.transform;

                _enemiesToSpawn--;
            }

            else if (_currentLevel == 2)
            {
                yield return new WaitForSeconds(3f);
                Vector3 spawnPos = new Vector3(Random.Range(9.2f, -9.2f), 8.17f, 0);
                Quaternion rotPos = Quaternion.Euler(0, 0, 180);

                GameObject newEnemy = Instantiate(_enemies[Random.Range(0, 2)], spawnPos, rotPos);
                newEnemy.transform.parent = _enemyContainer.transform;

                _enemiesToSpawn--;
            }

            else if (_currentLevel == 3)
            {
                yield return new WaitForSeconds(3f);
                Vector3 spawnPos = new Vector3(Random.Range(9.2f, -9.2f), 8.17f, 0);
                Quaternion rotPos = Quaternion.Euler(0, 0, 180);

                GameObject newEnemy = Instantiate(_enemies[Random.Range(0, 3)], spawnPos, rotPos);
                newEnemy.transform.parent = _enemyContainer.transform;

                _enemiesToSpawn--;
            }

            else if (_currentLevel == 4)
            {
                _enemiesToSpawn = 1;
                yield return new WaitForSeconds(3f);
                GameObject bossEnemy = Instantiate(_boss);
                _enemiesToSpawn = 0;
            }
            _isLevelEnding = true;
        }

    }

    IEnumerator SpawnBackgroundEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(3f);
            Vector3 spawnPos = new Vector3(Random.Range(9.2f, -9.2f), 14, 10);

            GameObject newEnemy = Instantiate(_backgroundEnemy, spawnPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(10.0f);
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(Random.Range(5f, 10f));
            Vector3 newPos = new Vector3(Random.Range(9.2f, -9.2f), 8.17f, 0);

            int randomPowerUp = Random.Range(0, 7);
            Instantiate(_powerUps[randomPowerUp], newPos, Quaternion.identity);
        }
    }

    IEnumerator SpawnCommonCollectableRoutine()
    {
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(10.0f);
            Vector3 newPos = new Vector3(Random.Range(9.2f, -9.2f), 8.17f, 0);

            GameObject _commonCollectable = Instantiate(_ammoCollectable, newPos, Quaternion.identity);
        }
    }

    IEnumerator SpawnRareCollectableRoutine()
    {
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(20f);
            Vector3 newPos = new Vector3(Random.Range(9.2f, -9.2f), 8.17f, 0);

            GameObject _rareCollectable = Instantiate(_healthCollectable, newPos, Quaternion.identity);
        }
    }

    private void NewLevel()
    {
        Instantiate(_astroid, new Vector3(0, 8, 0), Quaternion.identity);
        _isLevelEnding = false;
    }

    public void EnemyDestroyed(int _enemiesDestroyed)
    {
        _enemiesRemaining -= _enemiesDestroyed;
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }


}
