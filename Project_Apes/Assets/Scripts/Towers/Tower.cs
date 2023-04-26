using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum damageType
{
    fire,
    electric
}

public class Tower : MonoBehaviour
{
    //stats
    public float attackDelay;
    public damageType damageType;
    public float range = Mathf.Infinity;
    public float spawnOffset = 0.45f;
    //variables
    public bool engaged;

    public void DestroyTower()
    {
        //more stuff here later
        Destroy(gameObject);
    }

    public void Update()
    {
        DetectEnemies();
    }

    private void DetectEnemies()
    {
        GameObject closestEnemy = FindClosestEnemy();

        if (closestEnemy == null)
        {
            engaged = false;
            return;
        }

        var distance = Vector3.Distance(closestEnemy.transform.position, gameObject.transform.position);

        if (distance <= range)
            engaged = true;
        else
            engaged = false;
    }

    private GameObject FindClosestEnemy()
    {
        //get all enemies end return closest game object
        GameObject target = null;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        var distance = range;
        for (int i = 0; i < enemies.Length; i++)
        {
            var dist = Vector3.Distance(this.transform.position, enemies[i].transform.position);
            if (dist < distance)
            {
                target = enemies[i];
                distance = dist;
            }                
        }
        return target;
    }
}
