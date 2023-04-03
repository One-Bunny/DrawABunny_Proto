using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MushroomObject : MonoBehaviour
{
    [SerializeField] private float moveMultiple;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var target = other.transform;
            target.GetComponent<Rigidbody2D>().velocity =
                new Vector2(target.GetComponent<Rigidbody2D>().velocity.x, target.GetComponent<PlayerStatus>().data.jumpPower * moveMultiple);
        }
    }
}
