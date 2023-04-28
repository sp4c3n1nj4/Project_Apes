using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    TileManager manager;
    [SerializeField]
    GameObject WaveErrorMessage;

    [SerializeField]
    private GameObject Lane;
    private int waveIndex = 0;

    public UnityEvent waveStart;
    public List<GameObject> Lanes;

    private EnemySpawner[] spawners;
    private EnemyMovement[] movements;

    private void Awake()
    {
        Lanes = new List<GameObject>();
        if (waveStart == null)
            waveStart = new UnityEvent();
    }

    private void Start()
    {
        spawners = MakeSpawnerArray();
        movements = MakeMovementArray();

        CreateLane(new Vector2Int(0,13), EnemyType.wood, 10);
    }

    private void Update()
    {
        CheckWaveComplete();
    }

    private void CheckWaveComplete()
    {
        if (Lanes.Count! > 0)
            return;

        for (int i = 0; i < spawners.Length; i++)
        {
            if (!spawners[i].waveComplete)
                return;
        }
        WaveComplete();
    }

    private void CreateLane(Vector2Int pos, EnemyType _enemyType, int _enemyAmount, float _enemyOffset = 0.35f)
    {
        GameObject lane = Instantiate(Lane, manager.GetTile(pos), Quaternion.identity, transform);
        Lanes.Add(lane);

        lane.GetComponent<EnemyMovement>().startTile = pos;
        EnemySpawner spawner = lane.GetComponent<EnemySpawner>();
        spawner.enemyType = _enemyType;
        spawner.enemyAmount = _enemyAmount;
        spawner.enemyOffset = _enemyOffset;
    }

    public void TryAdvanceWave()
    {
        for (int i = 0; i < movements.Length; i++)
        {
            if (!movements[i].hasPath)
            {
                StartCoroutine(FailAdvanceWave());
                return;
            }               
        }

        AdvanceWaves();
    }

    private IEnumerator FailAdvanceWave()
    {
        WaveErrorMessage.SetActive(true);
        yield return new WaitForSeconds(3);
        WaveErrorMessage.SetActive(false);
    }

    private void AdvanceWaves()
    {
        spawners = MakeSpawnerArray();
        movements = MakeMovementArray();

        waveStart.Invoke();
    }

    private EnemySpawner[] MakeSpawnerArray()
    {
        List<EnemySpawner> l = new List<EnemySpawner>();
        for (int i = 0; i < Lanes.Count; i++)
        {
            l.Add(Lanes[i].GetComponent<EnemySpawner>());
        }
        return l.ToArray();
    }

    private EnemyMovement[] MakeMovementArray()
    {
        List<EnemyMovement> l = new List<EnemyMovement>();
        for (int i = 0; i < Lanes.Count; i++)
        {
            l.Add(Lanes[i].GetComponent<EnemyMovement>());
        }
        return l.ToArray();
    }

    private void WaveComplete()
    {
        waveIndex++;
    }
}
