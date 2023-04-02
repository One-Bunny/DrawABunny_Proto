using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCombine : MonoBehaviour
{
    public Image firstImage;
    public Image secondImage;

    public SpriteRenderer hammerSprite;

    public void GetRenderImage()
    {
        Texture2D texture1 = firstImage.sprite.texture;
        Texture2D texture2 = secondImage.sprite.texture;

        firstImage.gameObject.SetActive(false);
        secondImage.gameObject.SetActive(false);

        // texture의 크기는 무관하므로, 합친 이후에 사이즈를 조정해야 하는 것이 옳아보임.
        Texture2D newTexture = new Texture2D(texture2.width, (texture1.height + texture2.height));
        
        // texture1의 픽셀과 texture2의 픽셀을 합쳐준다. 그 전에는 alpha값을 투명으로 설정해야 함. -> texture2의 공간이 완벽하지 않기 때문임.
        for (var i = 0; i < newTexture.width; ++i)
        {
            for (var j = 0; j < newTexture.height; ++j)
            {
                newTexture.SetPixel(i, j, Color.clear);
            }
        }
        
        newTexture.SetPixels(0, (newTexture.height - texture2.height), texture2.width, texture2.height,
            texture2.GetPixels());
        newTexture.SetPixels((newTexture.width / 2) - (texture1.width / 2), 0, texture1.width, texture1.height,
            texture1.GetPixels());

        newTexture.Apply();

        Sprite newSprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height),
            new Vector2(0.5f, 0.5f));
        /*GetComponent<Image>().sprite = newSprite;
        GetComponent<Image>().rectTransform.sizeDelta = new Vector2(secondImage.rectTransform.sizeDelta.x, (secondImage.rectTransform.sizeDelta.y + firstImage.rectTransform.sizeDelta.y) - 1);*/
        hammerSprite.sprite = newSprite;
        hammerSprite.transform.gameObject.SetActive(true);
    }
}