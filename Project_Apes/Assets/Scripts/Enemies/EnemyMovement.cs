using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private TileManager manager;
    [SerializeField]
    private Pathfinding pathfinding;

    private Vector2Int[] Path;
    [SerializeField]
    private GameObject[] Enemies;

    [SerializeField]
    private float speed = 1, enemyOffset;
    private float percentage;

    public Vector2Int startTile;
    public Vector2Int endTile;
    public bool moveEnemies;

    private void Start()
    {
        DoPathfinding();

        manager.GridObstacleChange.AddListener(CheckPathfinding);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoPathfinding();
        }
    }

    public void StartWave()
    {
        percentage = 0;
        moveEnemies = true;
    }

    public void EndWave()
    {
        moveEnemies = false;
    }

    private void CheckPathfinding()
    {
        if (Path == null)
            return;


        for (int i = 0; i < Path.Length; i++)
        {
            if (manager.grid[Path[i].x, Path[i].y].walkable != true)
            {
                DoPathfinding();
                return;
            }
        }

        Vector2Int[] _path = pathfinding.FindPath(startTile, endTile);
        if (_path != null && _path.Length < Path.Length)
        {
            Path = _path;
        }
    }

    private void DoPathfinding()
    {
        Vector2Int[] _path = pathfinding.FindPath(startTile, endTile);
        if (_path != null)
        {
            Path = _path;
        }
        else
        {
            Path = new Vector2Int[0];
            PathfindingError();
        }
    }

    private void PathfindingError()
    {
        Debug.LogError("now valid path found");
    }

    private void FollowPath()
    {
        for (int i = 0; i < Enemies.Length; i++)
        {
            float newP = percentage - i * enemyOffset;
            if (newP >= Path.Length) continue;

            int s = Mathf.FloorToInt(newP);
            int e = s + 1;
            float p = newP - s;

            Vector3 pos = Vector3.Lerp(manager.GetTile(Path[s]), manager.GetTile(Path[e]), p);
            Enemies[i].transform.position = pos;
        }

        percentage += (1 / 50) * speed;
    }

    private void FixedUpdate()
    {
        if (!moveEnemies)
            return;

        FollowPath();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (Path != null)
        {
            for (int i = 0; i < Path.Length; i++)
            {
                Gizmos.DrawWireCube(manager.GetTile(Path[i]), new Vector3(.8f, .01f, .8f));

                if (i != 0)
                    Gizmos.DrawLine(manager.GetTile(Path[i - 1]), manager.GetTile(Path[i]));
            }
        }
    }
}
