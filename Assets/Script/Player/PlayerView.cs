using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    Animator anim;
    bool isMoving=false;
    bool isAttacking=false;
    bool isRunning=false;
    private void Awake()
    {
        anim = transform.GetChild(2).GetComponent<Animator>();
        //anim = GetComponent<Animator>();
    }
    public void PlayerMoving(bool value)
    {
        isMoving = value;
        anim.SetBool("IsMove", isMoving);
    }
    public void PlayerAttack()
    {
        if(isAttacking==false)
        {
            isAttacking = true;
            anim.SetTrigger("IsAttack");
        }
    }
    public void PlayerAttackEnd()
    {
        isAttacking = false;
    }
    public void PlayerRunning(bool value)
    {
        isRunning = value;
        anim.SetBool("IsRunning", value);
    }
}
