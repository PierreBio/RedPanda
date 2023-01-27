using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct EnemySpawn
{
    public GameObject go;
    public Transform goTf;
}

public class EnemySpawner : MonoBehaviour
{
    List<EnemySpawn> enemyList = new List<EnemySpawn>();
    [SerializeField] int maxEnemies = 200;
    [SerializeField] List<EnemySpawn> m_enemiesToSpawn;
    [SerializeField] float respawnCooldown = 2f;
    bool entered = false;


    void Start()
    {
        InvokeRepeating("SpawnEnemyWave", 5f, respawnCooldown);
    }
    public void SpawnEnemy(GameObject enemy, Transform enemyTf)
    {
        Instantiate(enemy, enemyTf.position, enemyTf.rotation);
    }

    void Update()
    {
        foreach (EnemySpawn enemySpawned in enemyList)
        {
            if ( enemySpawned.go == null) 
            {
                enemyList.Remove(enemySpawned);
            }
        }
    }

    public void SpawnEnemyWave()
    {
        SpawnEnemyWave(m_enemiesToSpawn);
    }

    public void SpawnEnemyWave(List<EnemySpawn> enemiesToSpawn)
    {
            if(enemyList.Count < maxEnemies)
            {
                foreach (EnemySpawn enemyToSpawn in enemiesToSpawn)
                {
                    if (enemyToSpawn.go != null && enemyToSpawn.goTf != null)
                    {
                        SpawnEnemy(enemyToSpawn.go, enemyToSpawn.goTf);
                        enemyList.Add(enemyToSpawn);
                    }
                }
            }
    }

    // void OnTriggerEnter(Collider collider)
    // {
    //     if (collider.CompareTag("Player") && !entered)
    //     {
    //         StartCoroutine(SpawnEnemyWave(m_enemiesToSpawn));
    //         entered = true;
    //     }
    // }
}
