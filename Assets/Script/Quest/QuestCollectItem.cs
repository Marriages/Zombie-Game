using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCollectItem : MonoBehaviour
{
    Transform collectableItem;
    GameObject collectEffect;
    Vector3 rotateDir;
    float rotateSpeed = 60f;
    PlayerQuest playerQuest;
    SphereCollider col;
    private void Awake()
    {
        collectableItem = transform.GetChild(0).transform;
        collectEffect = transform.GetChild(1).gameObject;
        rotateDir = new Vector3(0, rotateSpeed, 0);
        col = transform.GetComponent<SphereCollider>();
    }
    private void Update()
    {
        collectableItem.Rotate(rotateDir * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Collect..!");
            if(playerQuest==null)
                playerQuest = other.GetComponent<PlayerQuest>();

            playerQuest.CollectCheck();

            collectableItem.gameObject.SetActive(false);
            collectEffect.SetActive(true);
            col.enabled = false;
            Destroy(this.gameObject, 1f);
            //단순히 수집되었는지 아닌지 여부만 확인할 수 있게끔. 판단은 매니저들이
        }
    }
}
