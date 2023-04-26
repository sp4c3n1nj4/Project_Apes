using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodEnemy : Enemy
{
    public static new EnemyType enemyType = EnemyType.clay;

    public List<Send> drops;

    private void Start()
    {
        drops = new List<Send>();
    }

    public override void DestroyEnemy()
    {
        for (int i = 0; i < typesTaken.Count; i++)
        {
            switch (typesTaken[i])
            {
                case DamageType.fire:
                    drops.Add(new Send("shard", 3));
                    break;
                case DamageType.electric:
                    break;
                case DamageType.ice:
                    break;
                case DamageType.steam:
                    break;
                case DamageType.water:
                    break;
                case DamageType.magnet:
                    break;
            }
        }
    }
}
