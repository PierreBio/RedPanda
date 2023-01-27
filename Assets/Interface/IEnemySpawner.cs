using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemySpawner
{
    public void SpawnEnemy(GameObject enemy, Transform enemyTf);

    public void SpawnEnemyWave(Dictionary<GameObject, Transform> enemiesToSpawn );
}
