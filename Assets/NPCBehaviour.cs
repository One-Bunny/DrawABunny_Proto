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


    // Text Field를 가지고 있어야 한다. 또, Field에 출력해줄 Data를 가지고 있어야 함.
    [SerializeField] private Image _textField;
    [SerializeField] private TMP_Text _text;

    [SerializeField] private Canvas buttonInfoCanvas;



}