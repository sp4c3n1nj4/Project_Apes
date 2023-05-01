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
    GameObject WaveStartButton;

    [SerializeField]
    private GameObject Lane;
    private int waveIndex = 0;
    private bool waveOngoing = false;

    public UnityEvent waveStart;
    public List<GameObject> Lanes;
    public List<Vector2Int> LaneStarts;

    private EnemySpawner[] spawners;
    private EnemyMovement[] movements;

    private void Awake()
    {
        Lanes = new List<GameObject>();
        LaneStarts = new List<Vector2Int>();
        if (waveStart == null)
            waveStart = new UnityEvent();
    }

    private void Start()
    {
        spawners = MakeSpawnerArray();
        movements = MakeMovementArray();

        NewLane();
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

    private void NewLane()
    {
        if (waveIndex % 2 != 0)
            return;

        int _enemyAmount = Mathf.FloorToInt(waveIndex / 2) * 5 + 10;

        EnemyType _enemyType = EnemyType.wood;
        int randomType = UnityEngine.Random.Range(1, 5);
        switch (randomType)
        {
            case 1:
                break;
            case 2:
                _enemyType = EnemyType.metal;
                break;
            case 3:
                _enemyType = EnemyType.clay;
                break;
            case 4:
                _enemyType = EnemyType.plastic;
                break;
            default:
                break;
        }

        Vector2Int pos = Vector2Int.zero;
        int otherAxis = UnityEngine.Random.Range(0, manager.gridSize.x);
        int randomAxis = UnityEngine.Random.Range(1, 5);
        switch (randomAxis)
        {
            case 1:
                pos.x = 0;
                pos.y = otherAxis;
                break;
            case 2:
                pos.x = manager.gridSize.x - 1;
                pos.y = otherAxis;
                break;
            case 3:
                pos.y = 0;
                pos.x = otherAxis;
                break;
            case 4:
                pos.y = manager.gridSize.x - 1;
                pos.x = otherAxis;
                break;
            default:
                break;
        }

        CreateLane(pos, _enemyType, _enemyAmount);
    }

    public void TryAdvanceWave()
    {
        if (waveOngoing)
            return;

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

    public void TryEndWave()
    {
        if (!waveOngoing)
            return;

        for (int i = 0; i < Lanes.Count; i++)
        {
            if (!Lanes[i].GetComponent<EnemySpawner>().waveComplete)
                return;
        }

        EndWave();
    }

    private IEnumerator FailAdvanceWave()
    {
        WaveErrorMessage.SetActive(true);
        yield return new WaitForSeconds(3);
        WaveErrorMessage.SetActive(false);
    }

    private void AdvanceWaves()
    {
        waveOngoing= true;
        WaveStartButton.SetActive(false);

        spawners = MakeSpawnerArray();
        movements = MakeMovementArray();

        waveStart.Invoke();
    }

    private void EndWave()
    {
        waveOngoing = false;
        WaveStartButton.SetActive(true);
        waveIndex++;
        NewLane();
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
}
