using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveScript : MonoBehaviour
{
    [SerializeField] Wave[] waves;
    [SerializeField] EnemyType[] enemyTypes;
    [SerializeField] bool isEndless = false;

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
            if (gameUI.GetHealth() <= 0)
            {
                yield break;
            }

            yield return null;
            yield return new WaitUntil(() => !isWaveActive);

            if (currentWaveIndex != 0 && !isEndless)
            {
                gameUI.AddMoney(waves[currentWaveIndex - 1].endOfRoundReward);
            }
            else if (currentWaveIndex != 0 && isEndless)
            {
                gameUI.AddMoney(currentWaveIndex * 30);
            }

            if (currentWaveIndex >= waves.Length && !isEndless) { break; }

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
        if (waveIndex >= waves.Length)
        {
            int EnemyPoints = waveIndex * 10;
            while (EnemyPoints > 0)
            {
                EnemyType randomEnemy = enemyTypes[Random.Range(0, enemyTypes.Length)];
                if (randomEnemy.cost <= EnemyPoints)
                {
                    enemies.Add(Instantiate(randomEnemy.enemyPrefab, spawners[spawnerIndex].transform.position, Quaternion.identity));
                    EnemyPoints -= randomEnemy.cost;
                    spawnerIndex = (spawnerIndex + 1) % spawners.Length;
                    yield return new WaitForSeconds(randomEnemy.spawnRate);
                }
            }
        }
        else
        {
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
