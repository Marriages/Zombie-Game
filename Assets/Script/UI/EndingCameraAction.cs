using Cinemachine;
using UnityEngine;

public class EndingCameraAction : MonoBehaviour
{
    CinemachineDollyCart cart;
    CinemachineVirtualCamera cam;

    private void Awake()
    {
        cart = transform.GetChild(1).GetComponent<CinemachineDollyCart>();
        cam = transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>();
    }
    public void EndingCameraSetting()
    {
        cart.m_Speed = 2f;
        cam.Priority = 11;
    }
}
