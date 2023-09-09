using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EscapeCarController : MonoBehaviour
{
    Vector3[] movePos;
    NavMeshAgent agent;
    int posIndex = 0;
    BoxCollider col;

    private void Awake()
    {
        col=GetComponent<BoxCollider>();
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true;
        movePos = new Vector3[]  {transform.parent.GetChild(1).transform.position,
                                 transform.parent.GetChild(2).transform.position} ;
    }
    public void EndingCarStart()
    {
        col.enabled = false;
        agent.SetDestination(movePos[0]);
        posIndex++;
        agent.isStopped = false;
    }
    private void Update()
    {
        if (agent.isStopped == false && agent.remainingDistance < 0.1f)
        {
            agent.SetDestination(movePos[posIndex]);
            posIndex++;
            if(posIndex>= movePos.Length)
            {
                agent.isStopped = true;
                //EndingScene으로 넘어가게 할 것
            }
        }
    }
}
