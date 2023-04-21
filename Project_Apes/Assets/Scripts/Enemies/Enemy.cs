using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Entity
{
    private GameObject player;
    private CharacterController controller;
    public float attackRange;
    public float speed;
    
    public float attackDelay;
    public float attackDuration;
    public float damage;
    public int gReward;

    private float attackTimer = 0;
    private bool engaged;
    private float movementRange = Mathf.Infinity;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        controller = GetComponent<CharacterController>();
    }

    private GameObject FindClosestEnemy()
    {
        //get all enemies and return closest game object
        GameObject target = null;

        GameObject[] targets1 = GameObject.FindGameObjectsWithTag("Tower");
        GameObject[] targets2 = GameObject.FindGameObjectsWithTag("Player");

        GameObject[] targets = targets1.Concat(targets2).ToArray();

        var distance = movementRange;
        for (int i = 0; i < targets.Length; i++)
        {
            var dist = Vector3.Distance(this.transform.position, targets[i].transform.position);
            if (dist < distance)
            {
                target = targets[i];
                distance = dist;
            }                
        }

        return target;
    }

    private void FixedUpdate()
    {
        if (FindClosestEnemy() != null)
            MoveTowardsTarget(FindClosestEnemy());
    }

    private void MoveTowardsTarget(GameObject target)
    {
        controller.Move(Vector3.Normalize(target.transform.position - gameObject.transform.position) * Time.fixedDeltaTime * speed);
    }

    public override void Update()
    {
        base.Update();

        if (FindClosestEnemy() != null)
            DetectEnemies();

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        else if (engaged && attackTimer <= 0)
        {
            Attack();
            attackTimer = attackDelay;
        }
    }

    private void DetectEnemies()
    {
        var distance = Vector3.Distance(FindClosestEnemy().transform.position, gameObject.transform.position);

        if (distance <= attackRange)
            engaged = true;
        else
            engaged = false;
    }

    public virtual void Attack()
    {
        throw new NotImplementedException();
    }

    public override void DestroyEntity()
    {
        if (FindObjectOfType<ResourceManager>())
            FindObjectOfType<ResourceManager>().AddGold(gReward);

        base.DestroyEntity();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (FindClosestEnemy() != null)
            Gizmos.DrawLine(FindClosestEnemy().transform.position, gameObject.transform.position);
    }
}
