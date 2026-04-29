using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveScript : MonoBehaviour
{
    [SerializeField] Wave[] waves;

    List<GameObject> enemies = new List<GameObject>();
    GameObject[] spawners;
    bool isWaveActive = false;
    int currentWaveIndex = 0;
    UIScript gameUI;

    private void Start()
    {
        spawners = GameObject.FindGameObjectsWithTag("EnemySpawn");
        gameUI = FindFirstObjectByType<UIScript>();
        StartCoroutine(StartNewWave());
    }


    IEnumerator StartNewWave()
    {
        while (true)
        {
            

            yield return null;
            yield return new WaitUntil(() => !isWaveActive);

            if (currentWaveIndex != 0)
            {
                gameUI.AddMoney(waves[currentWaveIndex - 1].endOfRoundReward);
            }

            if (currentWaveIndex >= waves.Length) { break; }

            yield return new WaitForSeconds(2f);
            StartCoroutine(SpawnWave(currentWaveIndex));
            currentWaveIndex++;
        }
        Debug.Log("You Win");
    }

    IEnumerator SpawnWave(int waveIndex)
    {
        if (gameUI == null)
        {
            gameUI = FindFirstObjectByType<UIScript>();
        }

        gameUI.SetWave(waveIndex + 1);

        int spawnerIndex = 0;
        isWaveActive = true;
        yield return null;
        int amount = waves[waveIndex].enemies.Length;

        for (int i = 0; i < amount; i++)
        {
            int amountOfEnemies = waves[waveIndex].enemies[i].amount;
            for (int j = 0; j < amountOfEnemies; j++)
            {
                enemies.Add(Instantiate(waves[waveIndex].enemies[i].enemyPrefab, spawners[spawnerIndex].transform.position, Quaternion.identity));


                spawnerIndex = (spawnerIndex + 1) % spawners.Length;


                yield return new WaitForSeconds(waves[waveIndex].enemies[i].spawnRate);

            }
        }

        yield return new WaitUntil(() => AllEnemiesDead());

        isWaveActive = false;
    }

    public bool AllEnemiesDead()
    {
        enemies.RemoveAll(e => e == null); 
        return enemies.Count == 0;
    }

}
