using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    wood,
    metal,
    clay,
    plastic
}

public class Enemy : MonoBehaviour
{
    private PlayerHealth hp;

    public EnemyType enemyType;
    public List<DamageType> typesTaken;

    private void Awake()
    {
        hp = GameObject.FindObjectOfType<PlayerHealth>();
    }

    public virtual void ReachedEnd()
    {       
        print("reached end");
        DestroyEnemy();
        hp.TakeDamage(1);
    }

    public virtual void DestroyEnemy()
    {
        gameObject.GetComponentInParent<EnemySpawner>().enemies.Remove(gameObject);
        Destroy(gameObject);
    }

    public void TakeDamage(DamageType damageType)
    {
        print("got hit with: " + damageType.ToString());
        typesTaken.Add(damageType);
        CheckTypes();
    }

    public void CheckTypes()
    {
        for (int i = 0; i < typesTaken.Count; i++)
        {
            switch (typesTaken[i])
            {
                case DamageType.saw:
                    DestroyEnemy();
                    break;
                case DamageType.crush:
                    DestroyEnemy();
                    break;
                case DamageType.drill:
                    DestroyEnemy();
                    break;
                case DamageType.laser:
                    DestroyEnemy();
                    break;
                default:
                    break;
            }
        }
    }
}
