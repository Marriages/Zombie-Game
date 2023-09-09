using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCubeDelete : MonoBehaviour
{
    bool cubeDelete = false;
    public void TutorialCubeDisappear()
    {
        cubeDelete = true;
    }
    private void Update()
    {
        if(cubeDelete)
        {
            transform.Translate(Vector3.down * 3 * Time.deltaTime);
            if(transform.position.y<-5f)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
