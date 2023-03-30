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


    // Text Field�� ������ �־�� �Ѵ�. ��, Field�� ������� Data�� ������ �־�� ��.
    [SerializeField] private Image _textField;
    [SerializeField] private TMP_Text _text;

    [SerializeField] private Canvas buttonInfoCanvas;

    private void Awake()
    {
        playerLayer = LayerMask.GetMask("Player");
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        if (SearchPlayer())
        {
            buttonInfoCanvas.gameObject.SetActive(true);
        }
        else
        {
            buttonInfoCanvas.gameObject.SetActive(false);
        }

    }

    private bool SearchPlayer()
    {
        // Search Player

        bool _isPlayerChecked = Physics2D.OverlapCircle(transform.position, _searchRange);

        if(_isPlayerChecked)
        {
            return true;
        }
        return false;
    }
}
