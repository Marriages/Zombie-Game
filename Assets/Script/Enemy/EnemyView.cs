using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    Animator anim;
    public GameObject hitEffect;
    private void Awake()
    {
        anim=GetComponent<Animator>();
        if(hitEffect==null)
        {
            hitEffect = transform.GetChild(4).gameObject;
            hitEffect.SetActive(false);
        }
    }
    private void Start()
    {
        hitEffect.SetActive(false);
    }


    public void EnemyDetect(bool isDetect)
    {
        anim.SetBool("IsDetect", isDetect);
    }
    public void EnemyAttack()
    {
        anim.SetTrigger("IsAttack");
    }
    public void EnemyChase(bool isChase)
    {
        anim.SetBool("IsChase", isChase);
    }
    public void EnemyWakeUp()
    {
        anim.SetTrigger("IsWakeUp");
    }
    public void EnemyDie(Vector3 hitPosition)
    {
        hitEffect.transform.position = hitPosition;
        hitEffect.SetActive(true);
        anim.SetTrigger("IsDie");
        StartCoroutine(EnemyDieEffect());
    }

    IEnumerator EnemyDieEffect()
    {
        //Debug.Log("EnemyDieEffect...!");
        //몬스터가 죽을 때 사라지는 효과를 구현하는 곳
        yield return new WaitForSeconds(8f);
        transform.gameObject.SetActive(false);
    }
}
