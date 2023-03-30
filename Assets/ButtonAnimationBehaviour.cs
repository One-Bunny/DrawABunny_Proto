using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonAnimationBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.pointerEnter.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}