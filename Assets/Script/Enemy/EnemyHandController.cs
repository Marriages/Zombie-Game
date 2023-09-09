using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandController : MonoBehaviour
{
    SphereCollider handCol;

    private void Start()
    {
        handCol = GetComponent<SphereCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //Debug.Log("Enemy's hand Contect!");
            other.GetComponent<ThirdPersonController>().PlayerOnHit();
            handCol.enabled = false;
        }
    }
}
