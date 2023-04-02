using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewButtonRange : MonoBehaviour
{
    public Canvas viewCanvas = null;
    public float viewRange = .0f;

    public GameObject spawnPoint;

    private void Awake()
    {
        if (viewCanvas == null)
        {
            Debug.LogError($"{viewCanvas.GetType()}이 존재하지 않습니다.");
        }
    }

    private void Update()
    {
        Search();
    }

    private void Search()
    {
        var target = Physics2D.BoxCast(spawnPoint.transform.position, new Vector2(viewRange, viewRange), 0f,
            Vector2.right, 1f, LayerMask.GetMask("Player"));
        
        viewCanvas.gameObject.SetActive(target.collider != null ? true : false);

        DebugDrawBox(spawnPoint.transform.position, new Vector2(viewRange, viewRange), Color.red);
    }

    void DebugDrawBox(Vector2 center, Vector2 size, Color color)
    {
        Vector2 topLeft = center - size / 2;
        Vector2 topRight = new Vector2(topLeft.x + size.x, topLeft.y);
        Vector2 bottomLeft = new Vector2(topLeft.x, topLeft.y + size.y);
        Vector2 bottomRight = new Vector2(topLeft.x + size.x, topLeft.y + size.y);

        Debug.DrawLine(topLeft, topRight, color);
        Debug.DrawLine(topRight, bottomRight, color);
        Debug.DrawLine(bottomRight, bottomLeft, color);
        Debug.DrawLine(bottomLeft, topLeft, color);
    }
}