using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private WaveManager manager;
    [SerializeField]
    private GameObject[] enemyPrefabs;

    //stats
    public EnemyType enemyType;
    public float enemyOffset;
    public int enemyAmount;
    public float speed = 1;

    //vars
    public List<GameObject> enemies;
    public bool waveComplete = false; 

    private void Start()
    {
        manager = GameObject.FindObjectOfType<WaveManager>();
        manager.waveStart.AddListener(StartWave);
    }

    private void Update()
    {
        if (waveComplete)
            return;

        if (enemies.Count == 0)
        {
            LaneComplete();
        }
    }

    public void StartWave()
    {
        waveComplete = false;
        StartCoroutine(SpawnEnemies());
        gameObject.GetComponent<EnemyMovement>().StartWave(enemyOffset, speed);
    }

    private void LaneComplete()
    {
        waveComplete = true;
        manager.TryEndWave();
    }

    private GameObject EnemyPrefab()
    {
        int i = 0;
        switch (enemyType)
        {            
            case EnemyType.wood:
                i = 0;
                break;
            case EnemyType.metal:
                i = 1;
                break;
            case EnemyType.clay:
                i = 2;
                break;
            case EnemyType.plastic:
                i = 3;
                break;
            default:
                break;
        }
        return enemyPrefabs[i];
    }

    IEnumerator SpawnEnemies()
    {
        enemies = new List<GameObject>();
        for (int i = 1; i < enemyAmount; i++)
        {
            GameObject enemy = Instantiate(EnemyPrefab(), transform);
            enemies.Add(enemy);           
            yield return new WaitForSeconds(enemyOffset / speed);
        }
    }
}
