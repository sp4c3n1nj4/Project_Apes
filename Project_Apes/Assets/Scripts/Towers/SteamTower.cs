using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SteamTower : Tower
{
    private int tilerange = 3 ;

    private void Start()
    {
        damageType = DamageType.fire;
        range = 2f;
        attackDelay = 0.5f;
    }

    public override void Attack()
    {
        RaycastHit[] hits;
        RaycastHit[] hits1;
        RaycastHit[] hits2;
        hits1 = Physics.RaycastAll(TileOffsetPosition(1, -1.5f), TileOffsetPosition(1, 1.5f), tilerange + 1, LayerMask.GetMask("Enemy"));
        hits2 = Physics.RaycastAll(TileOffsetPosition(-1, -1.5f), TileOffsetPosition(-1, 1.5f), tilerange + 1, LayerMask.GetMask("Enemy"));

        hits = hits1.Concat(hits2).ToArray();
        for (int i = 0; i < hits.Length; i++)
        {
            hits[i].transform.gameObject.GetComponent<Enemy>().TakeDamage(damageType);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(TileOffsetPosition(1, -1.5f), TileOffsetPosition(1, 1.5f));
        Gizmos.DrawLine( TileOffsetPosition(-1, -1.5f),TileOffsetPosition(-1, 1.5f));
    }

}
