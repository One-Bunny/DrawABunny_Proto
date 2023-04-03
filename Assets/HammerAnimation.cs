using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HammerAnimation : MonoBehaviour
{
    public GameObject wall;
    public GameObject fakeWall;
    
    public void ActiveHammer()
    {
        Debug.Log("ACTIVE HAMMER!");
        
        // 해머 자기자신 삭제 및 Wall 삭제
        Destroy(this.gameObject);
        fakeWall.SetActive(true);
        Destroy(wall.gameObject);
    }
}
