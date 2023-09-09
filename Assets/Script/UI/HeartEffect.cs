using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartEffect : MonoBehaviour
{
    Color c;
    float heartSpeed = 0.05f;

    private void Start()
    {
        c = GetComponent<Image>().color;
    }

    public void HeartChange(bool active)
    {
        if(active)
        {
            StartCoroutine(HeartAppearEffect());
        }
        else
        {
            StartCoroutine(HeartDisappearEffect());
        }
    }


    IEnumerator HeartDisappearEffect()
    {
        Color tc = c;
        while (c.a > 0.1f)
        {
            tc.a -= heartSpeed;
            c = tc;
            yield return null;
        }
        c.a = 0;
    }
    IEnumerator HeartAppearEffect()
    {
        Color tc = c;
        while (c.a < 1f)
        {
            tc.a += heartSpeed;
            c = tc;
            yield return null;
        }
        c.a = 1;
    }
}
