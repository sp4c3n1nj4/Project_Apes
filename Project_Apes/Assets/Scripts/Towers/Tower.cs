using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum DamageType
{
    saw,
    crush,
    drill,
    laser,
    fire,
    ice,
    electric,
    steam,
    water,
}

public class Tower : MonoBehaviour
{
    //stats
    [DoNotSerialize]
    public float attackDelay;
    [DoNotSerialize]
    public DamageType damageType;
    public float range = Mathf.Infinity;
    public float spawnOffset = 0.45f;
    public Resource[] cost;
    //variables
    public bool engaged;
    private float delay;

    public void DestroyTower()
    {
        
        Destroy(gameObject);
    }

    public virtual void Attack()
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        DetectEnemies();

        delay += Time.deltaTime;
        delay = Mathf.Clamp(delay,0, attackDelay);
        if (engaged && delay >= attackDelay)
        {
            Attack();
            delay= 0;
        }
    }

    public Vector3 TileOffsetPosition(Vector2Int offset)
    {
        Vector3 pos = transform.position;

        pos.x += offset.x;
        pos.z += offset.y;

        return pos;
    }
    public Vector3 TileOffsetPosition(float offsetX, float offsetY)
    {
        Vector3 pos = Vector3.zero;

        pos.x += offsetX;
        pos.z += offsetY;

        pos = transform.rotation * pos;
        pos += transform.position;

        pos.y = 0.1f;

        return pos;
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
