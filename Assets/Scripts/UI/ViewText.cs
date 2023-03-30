using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewText : MonoBehaviour
{
    public float _searchRange = .0f;

    private void Awake()
    {
    }

    private void FixedUpdate()
    {
        var isOn = Physics2D.OverlapCircle(transform.position, _searchRange, LayerMask.GetMask("Player"));

        if(isOn)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _searchRange);
    }
#endif
}
