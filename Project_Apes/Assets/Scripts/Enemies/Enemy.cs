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
    public EnemyType enemyType;
    public List<DamageType> typesTaken;

    //public float health;
    //public float maxHealth;
    //public virtual void Update()
    //{
    //    if (health <= 0)
    //    {
    //        DestroyEntity();
    //    }
    //}

    public virtual void DestroyEnemy()
    {       
        //Destroy(gameObject);
    }

    public void TakeDamage(DamageType damageType)
    {
        typesTaken.Add(damageType);
        CheckTypes();
    }

    public void CheckTypes()
    {
        for (int i = 0; i < typesTaken.Count; i++)
        {
            switch (typesTaken[i])
            {
                case DamageType.cut:
                    DestroyEnemy();
                    break;
                case DamageType.press:
                    DestroyEnemy();
                    break;
                case DamageType.shredd:
                    DestroyEnemy();
                    break;
                default:
                    break;
            }
        }
    }
}
