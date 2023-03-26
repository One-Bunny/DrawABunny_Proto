using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneBunny
{
    [Serializable]
    public class BGInfoArray
    {
        public Renderer renderer;
        public float scrollSpeed;
        public float materialOffsetX;
    }

    public class BackgroundScroller : MonoBehaviour
    {
        [field:SerializeField] private BGInfoArray[] ScrollSprite { get; set; }

        private void Start()
        {
            if (ScrollSprite.Length is < 0 or 0)
            {
                Debug.LogError($"{ScrollSprite.GetType().Name}이 존재하지 않습니다.");
                Debug.Break();
            }
        }

        private void Update()
        {
            ScrollMove();
        }

        private void ScrollMove()
        {
            for (int i = 0; i < ScrollSprite.Length; i++)
            {
                SetTextureOffset(ScrollSprite[i]);
            }
        }

        private void SetTextureOffset(BGInfoArray scrollData)
        {
            scrollData.materialOffsetX += (float)(scrollData.scrollSpeed) * Time.deltaTime;
            
            if (scrollData.materialOffsetX >= 1)
                scrollData.materialOffsetX %= 1.0f;

            var offset = new Vector2(scrollData.materialOffsetX, 0);

            scrollData.renderer.material.SetTextureOffset("_MainTex", offset);
        }
    }
}