using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFrontLook : MonoBehaviour
{
    public Transform playerBody;

    private void Update()
    {
        transform.forward = playerBody.forward;
    }
}
