using System;
using System.Collections.Generic;
using Script;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public List<GameObject> _enemyPool;
    private PlayerMovement _playerMovement;
    public int poolCount;
    public GameObject enemyPrefab;
    public int liveSceneEnemy;
    private float currentTime;
    public float delayTime;
    public int liveEnemyCurrentStage;
  
    private void Awake()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
        _enemyPool = new List<GameObject>();
    }

    void Start()
    {
        EnemyPoolAdd(poolCount);
    }

    public void EnemyPoolAdd(int index)
    {
        for (int i = 0; i < index; i++)
        {
            GameObject currentEnemy = Instantiate(enemyPrefab);
            currentEnemy.SetActive(false);
        }
    }
    
    public void CallEnemy()
    {
        if (_enemyPool.Count > 0)
        {
            GameObject currentEnemy = _enemyPool[0];
            currentEnemy.SetActive(true);
            currentEnemy.GetComponent<Enemy>().Live();
            _enemyPool.RemoveAt(0);
        }
        else
        {
            EnemyPoolAdd(5);
        }
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime > delayTime)
        {
            EnemySpawnScene();
            currentTime = 0;
        }
    }

    public void EnemySpawnScene()
    {
        if (liveSceneEnemy<liveEnemyCurrentStage)
        {
            CallEnemy();
        }
    }

    public void LiveEnemyAdd()
    {
        liveSceneEnemy++;
    }
    public void DeadEnemyRemove()
    {
        liveSceneEnemy--;
    }
}
