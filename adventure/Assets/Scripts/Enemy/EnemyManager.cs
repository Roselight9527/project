using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    private ObjectPool<GameObject> enemyPool1;
    private ObjectPool<GameObject> enemyPool2;
    private EnemyCheck point1;
    private bool hasSpawned = false; // 标记是否已经生成过敌人

    void Start()
    {
        // 初始化对象池
        enemyPool1 = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(enemyPrefab1),
            actionOnGet: enemy => enemy.SetActive(true),
            actionOnRelease: enemy => enemy.SetActive(false),
            actionOnDestroy: Destroy,
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 20
        );

        enemyPool2 = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(enemyPrefab2),
            actionOnGet: enemy => enemy.SetActive(true),
            actionOnRelease: enemy => enemy.SetActive(false),
            actionOnDestroy: Destroy,
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 20
        );

        point1 = GetComponent<EnemyCheck>();
        if (point1 == null)
        {
            Debug.LogError("EnemyCheck component not found on the GameObject or its children/parent!");
        }
    }

    public GameObject GetEnemy(int type)
    {
        if (type == 1)
        {
            return enemyPool1.Get();
        }
        else if (type == 2)
        {
            return enemyPool2.Get();
        }
        else
        {
            return null;
        }
    }

    public void ReleaseEnemy(GameObject enemy, int type)
    {
        if (type == 1)
        {
            enemyPool1.Release(enemy);
        }
        else if (type == 2)
        {
            enemyPool2.Release(enemy);
        }
    }

    void SpawnEnemy(int type)
    {
        GameObject enemy = GetEnemy(type);
        if (enemy != null)
        {
            enemy.transform.position = new Vector3(145, -12, 0);
        }
    }

    public void EnemyCreate()
    {
        if (point1 != null && point1.isEnterned && !hasSpawned)
        {
            SpawnEnemy(1);
            hasSpawned = true; // 设置标志位，避免重复生成
        }
    }

    private void Update()
    {
        EnemyCreate();
    }
}
