using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCBehaviour : MonoBehaviour
{
    // Player�� �����ϸ� Text�� ����� �Ѵ�. -> Range �ʿ� �� Player �ν� �ʿ�
    [field: SerializeField] public float _searchRange { get; private set; }

    private LayerMask playerLayer;

    [SerializeField] private TMP_Text _text;
}