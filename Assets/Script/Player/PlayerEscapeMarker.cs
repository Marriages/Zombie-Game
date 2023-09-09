using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEscapeMarker : MonoBehaviour
{
    public EscapeArea escapeArea;
    Vector3 escapeAreaDir;
    MeshRenderer[] arrows;

    private void Awake()
    {
        if (escapeArea == null)
            escapeArea = FindObjectOfType<EscapeArea>();
        arrows = transform.GetComponentsInChildren<MeshRenderer>();
    }
    private void Update()
    {
        escapeAreaDir = (escapeArea.transform.position - transform.parent.position).normalized;
        transform.position = transform.parent.position + escapeAreaDir+Vector3.up;
        transform.forward = escapeAreaDir;
    }
}
