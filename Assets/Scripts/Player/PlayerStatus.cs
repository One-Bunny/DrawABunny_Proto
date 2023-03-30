using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public PlayerStatusData data;

    private void Awake()
    {
        if (data == null)
        {
            Debug.LogError($"{data.GetType()}가 존재하지 않습니다.");
        }
    }
}
