using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    //몬스터당 공격을 한번씩만 수행할 수 있도록 조치가 필요.
    // 공격 시작때만 해당 콜라이더가 켜질 수 있도록.
    public bool isAttacking=false;
    BoxCollider playerWeaponCol;
    public Action PlayerWeaponHit;

    private void Start()
    {
        playerWeaponCol=GetComponent<BoxCollider>();
    }
    public void AttackProcessing(bool condition)
    {
        isAttacking = condition;
        playerWeaponCol.enabled = isAttacking;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            PlayerWeaponHit?.Invoke();
            other.GetComponent<EnemyController>().EnemyHitByPlayer(other.ClosestPointOnBounds(transform.position));
        }
    }



}
