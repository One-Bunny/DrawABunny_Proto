using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCBehaviour : MonoBehaviour
{
    // Player가 접근하면 Text를 띄워야 한다. -> Range 필요 및 Player 인식 필요
    [field: SerializeField] public float _searchRange { get; private set; }

    private LayerMask playerLayer;

    [SerializeField] private TMP_Text _text;
}