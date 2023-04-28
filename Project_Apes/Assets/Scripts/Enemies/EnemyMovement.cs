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
    [SerializeField]
    private EnemySpawner spawner;

    private Vector2Int[] Path;
   
    private float percentage;

    public float enemyOffset;
    public float speed = 1;

    public bool hasPath = false;

    public Vector2Int startTile;
    public Vector2Int endTile;
    public bool moveEnemies;

    private void Start()
    {
        manager = GameObject.FindObjectOfType<TileManager>();
        pathfinding = GameObject.FindObjectOfType<Pathfinding>();

        spawner = gameObject.GetComponent<EnemySpawner>();

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

    public void StartWave(float _enemyOffset, float _speed)
    {
        percentage = 0;
        moveEnemies = true;
        enemyOffset = _enemyOffset;
        speed = _speed;
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
            hasPath = true;
        }
    }

    private void DoPathfinding()
    {
        Vector2Int[] _path = pathfinding.FindPath(startTile, endTile);
        if (_path != null)
        {
            Path = _path;
            hasPath = true;
        }
        else
        {
            Path = new Vector2Int[0];
            hasPath = false;
            PathfindingError();
        }
    }

    private void PathfindingError()
    {
        Debug.LogError("now valid path found");
    }

    private void FollowPath()
    {
        GameObject[] Enemies = spawner.enemies.ToArray();
        print(Enemies.Length.ToString());
        for (int i = 0; i < Enemies.Length; i++)
        {
            float newP = percentage - i * enemyOffset;
            if (newP >= Path.Length)
                Enemies[i].GetComponent<Enemy>().ReachedEnd();
            newP = Mathf.Clamp(newP, 0, Path.Length - 1.001f);

            int s = Mathf.FloorToInt(newP);
            int e = s + 1;
            float p = newP - s;

            Vector3 pos = Vector3.Lerp(manager.GetTile(Path[s]), manager.GetTile(Path[e]), p);
            Enemies[i].transform.position = pos;
        }
        print(percentage);
        percentage += speed / 50;
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
