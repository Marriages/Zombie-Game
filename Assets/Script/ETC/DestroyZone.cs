using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyZone : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject!=null)
            Destroy(other.gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != null)
            Destroy(collision.gameObject);
    }
}
