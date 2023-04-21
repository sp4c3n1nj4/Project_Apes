using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private TileManager manager;

    private Vector2Int[] Path;
    [SerializeField]
    private GameObject[] Enemies;

    [SerializeField]
    private float speed = 1, enemyOffset;
    private float percentage;

    public void StartWave()
    {
        Path = manager.EnemyPath.ToArray();
        //Path.Reverse();
        percentage = 0;
    }

    private void FixedUpdate()
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
}
