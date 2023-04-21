using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEnemy : Enemy
{
    [SerializeField]
    private GameObject hitBox;

    public override void Attack()
    {
        StartCoroutine(AttackCoroutine(attackDuration));
    }

    IEnumerator AttackCoroutine(float duration)
    {
        hitBox.SetActive(true);
        yield return new WaitForSeconds(duration);
        hitBox.SetActive(false);
    }
}
