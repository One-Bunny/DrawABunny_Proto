using System.Collections;
using UnityEngine;

public class CircleWipeController : MonoBehaviour
{

    [SerializeField] private Shader _circleWipeShader;
    private Material _material;

    [Range(0, 1.2f)]
    [SerializeField]private float _radius = 0f;

    private float _horizontal = 16f;
    private float _vertical = 9f;
    private float _duration = 1f;


    void Start()
    {
        _material = new(_circleWipeShader);
        _radius = 1.2f;
        UpdateShader();
        FadeIn();
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, _material);
    }

    public void FadeOut()
    {
        StartCoroutine(DoFade(1.2f, 0f));
    }

    public void FadeIn()
    {
        StartCoroutine(DoFade(0, 1.2f));
    }

    IEnumerator DoFade(float start, float end)
    {
        _radius = start;
        UpdateShader();

        var time = 0f;
        while (time < 1f)
        {
            _radius = Mathf.Lerp(start, end, time);
            time += Time.deltaTime / _duration;
            UpdateShader();
            Debug.Log("Fade / "+ _radius+" / "+time);
            yield return null;
        }

        _radius = end;
        UpdateShader();
    }

    private void UpdateShader()
    {
        var _speed = Mathf.Max(_horizontal, _vertical);
        _material.SetFloat("_Horizontal", _horizontal);
        _material.SetFloat("_Vertical", _vertical);
        _material.SetFloat("_Speed", _speed);
        _material.SetFloat("_Radius", _radius);

        Debug.Log("UpdateShader / "+_radius);
    }


}
