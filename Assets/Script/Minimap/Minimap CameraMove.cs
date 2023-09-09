using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraMove : MonoBehaviour
{
    public GameObject targetPlayer;
    Vector3 targetPosition;
    private void Update()
    {
        targetPosition = new(targetPlayer.transform.position.x,60, targetPlayer.transform.position.z);
        transform.position = targetPosition;
    }
}
