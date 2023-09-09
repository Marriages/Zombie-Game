using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeArea : MonoBehaviour
{
    QuestManager qm;
    bool escape = false;
    private void Awake()
    {
        qm=FindObjectOfType<QuestManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && escape==false)
        {
            escape = true;
            qm.CompleteQuest();
            this.gameObject.SetActive(false);
        }
    }
}
