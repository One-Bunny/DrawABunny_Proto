using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadCanvas;
    public Image loadingFillBar;
    public Image bunnyImage;

    public TMP_Text percentText;

    private void Awake()
    {
        if (loadCanvas == null)
        {
            Debug.LogError($"{loadCanvas.GetType()} is NOT FOUND");
        }

        if (bunnyImage == null)
        {
            Debug.LogError($"{bunnyImage.GetType()} is NOT FOUND");
        }

        if (percentText == null)
        {
            Debug.LogError($"{percentText.GetType()} is NOT FOUND");
        }
    }

    public void LoadScene(int sceneID)
    {
        loadCanvas.SetActive(true);
        
        StartCoroutine(LoadAsyncScene(sceneID));
    }

    private void MoveBunny(float value)
    {
        var bunnyXPos = value * (781f * 2);
        bunnyXPos += bunnyImage.rectTransform.localPosition.x;
        
        bunnyImage.rectTransform.localPosition = new Vector3(bunnyXPos, bunnyImage.rectTransform.localPosition.y, 0);
    }

    private IEnumerator LoadAsyncScene(int sceenId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceenId);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / .9f);
            MoveBunny(progressValue);
            
            percentText.text = $"( {progressValue * 100f}% / 100% )";
            
            loadingFillBar.fillAmount = progressValue;

            yield return null;
        }
    }

}
