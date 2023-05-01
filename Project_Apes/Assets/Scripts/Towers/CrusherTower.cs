using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CrusherTower : Tower
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
        hits = Physics.RaycastAll(TileOffsetPosition(-1.5f, 1), TileOffsetPosition(1.5f, 1), tilerange + 1, LayerMask.GetMask("Enemy"));

        for (int i = 0; i < hits.Length; i++)
        {
            hits[i].transform.gameObject.GetComponent<Enemy>().TakeDamage(damageType);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(TileOffsetPosition(-1.5f, 1), TileOffsetPosition(1.5f, 1));
    }

}
