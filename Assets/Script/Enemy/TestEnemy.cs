using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    float hitTime;
    float hitInterval = 0.2f;
    float hp = 3;
    public float HP
    {
        get => hp;
        set
        {
            hp = value;
            Debug.Log("Enemy hit!");
            if(value<=0)
            {
                Destroy(this.gameObject);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="PlayerWeapon")
        {
            if(Time.time-hitTime>hitInterval)
            {
                hitTime = Time.time;
                HP--;
            }
        }
    }
}
