using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyPlayerDetector : MonoBehaviour
{
    // 바로 위가 본체여야합니다.
    public Action<bool> PlayerDetect;
    public SphereCollider col;
    public float detectRange = 5f;

    private void Awake()
    {
        col=transform.GetComponent<SphereCollider>();
    }
    private void Start()
    {
        col.radius = detectRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerDetect?.Invoke(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerDetect?.Invoke(false);
        }
    }
}
