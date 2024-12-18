using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject[] _powerUps;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private bool _stopSpawning;


    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());

        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_enemyPrefab == true)
        {
            yield return new WaitForSeconds(3f);
            Vector3 spawnPos = new Vector3(Random.Range(9.2f, -9.2f), 8.17f, 0);

            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
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

            int randomPowerUp = Random.Range(0, 3);
            Instantiate(_powerUps[randomPowerUp], newPos, Quaternion.identity);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }


}
