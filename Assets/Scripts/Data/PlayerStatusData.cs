using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatusData", menuName = "Data/PlayerStatusData", order = 0)]
public class PlayerStatusData : ScriptableObject
{
    public float moveSpeed;
    public float jumpPower;
}
