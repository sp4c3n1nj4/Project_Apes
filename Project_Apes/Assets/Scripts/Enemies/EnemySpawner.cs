using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private WaveManager manager;
    [SerializeField]
    private GameObject[] enemyPrefabs;
    [SerializeField]
    private GameObject[] enemyIcons;
    private TextMeshProUGUI text;

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
        enemyIcons[EnemyPrefabIndex()].SetActive(true);
        text.text = enemyAmount.ToString();
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

    private int EnemyPrefabIndex()
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
        return i;
    }

    IEnumerator SpawnEnemies()
    {
        enemies = new List<GameObject>();
        for (int i = 1; i < enemyAmount; i++)
        {
            GameObject enemy = Instantiate(enemyPrefabs[EnemyPrefabIndex()], transform);
            enemies.Add(enemy);           
            yield return new WaitForSeconds(enemyOffset / speed);
        }
    }
}
