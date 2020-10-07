using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private bool _stopSpawning = false;
    //[SerializeField]
    //private GameObject _powerUpPrefab; not needed created power up array to spawn
    public GameObject[] powerUps;
    

    // Start is called before the first frame update
    void Start()
    {
       
    }
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            float randomX = Random.Range(-9f, 9f);
            Vector3 spawnPos = new Vector3(randomX, 8f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(5);
        }


    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            float randomX = Random.Range(-9f, 9f);
            Vector3 spawnPos = new Vector3(randomX, 8f, 0);
            float randomSpawn = Random.Range(3.0f, 8.0f);
            int randomePowerUpSpawn = Random.Range(0, 4);
            yield return new WaitForSeconds(randomSpawn);
            Instantiate(powerUps[randomePowerUpSpawn], spawnPos, Quaternion.identity);
        }
        
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}