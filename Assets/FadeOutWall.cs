using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutWall : MonoBehaviour
{
    public SpriteRenderer render1;
    public SpriteRenderer render2;

    public void Awake()
    {
        StartCoroutine(FadeOut(render1, render2));
        
        Destroy(this.gameObject, 5f);
    }

    IEnumerator FadeOut(SpriteRenderer spr1, SpriteRenderer spr2)
    {
        float startTime = Time.time;
        Color color1 = spr1.color;
        Color color2 = spr1.color;
        while (Time.time < startTime + 2)
        {
            float t = (Time.time - startTime) / 2f;
            color1.a = Mathf.Lerp(1f, 0f, t);
            color2.a = Mathf.Lerp(1f, 0f, t);
            spr1.color = color1;
            spr2.color = color2;
            yield return null;
        }
        color1.a = 0f;
        color2.a = 0f;
        
        spr1.color = color1;
        spr2.color = color2;
    }
}
