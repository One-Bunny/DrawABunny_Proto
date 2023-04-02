using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DrawBehaviour : MonoBehaviour, IDragHandler, IEndDragHandler {
    Image drawImage;
    Sprite drawSprite;
    Texture2D drawTexture;

    Vector2 previousDragPosition;

    Color[] resetColorsArray;
    Color resetColor;

    Color32[] currentColors;

    RectTransform rectTransform;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
        drawImage = GetComponent<Image>();

        resetColor = new Color(0, 0, 0, 0);

        Initialize();

        //Toggle this off to prevent the texture from getting cleared
        ResetTexture();
    }

    public void Initialize() {
        drawSprite = drawImage.sprite;
        drawTexture = drawSprite.texture;

        // fill the array with our reset color so it can be easily reset later on
        resetColorsArray = new Color[(int)drawSprite.rect.width * (int)drawSprite.rect.height];
        for (int x = 0; x < resetColorsArray.Length; x++)
            resetColorsArray[x] = resetColor;
    }
    public void Paint(Vector2 pixelPosition) {
        currentColors = drawTexture.GetPixels32();

        if (previousDragPosition == Vector2.zero) {
            MarkPixelsToColour(pixelPosition);
        } else {
            ColorBetween(previousDragPosition, pixelPosition);
        }
        ApplyCurrentColors();

        previousDragPosition = pixelPosition;
    }

    public void MarkPixelsToColour(Vector2 centerPixel) {
        int centerX = (int)centerPixel.x;
        int centerY = (int)centerPixel.y;

        for (int x = centerX - 25; x <= centerX + 25; x++) {
            if (x >= (int)drawSprite.rect.width || x < 0)
                continue;

            for (int y = centerY - 25; y <= centerY + 25; y++) {
                MarkPixelToChange(x, y);
            }
        }
    }

    public void ColorBetween(Vector2 startPoint, Vector2 endPoint) {
        float distance = Vector2.Distance(startPoint, endPoint);

        Vector2 cur_position = startPoint;

        float lerp_steps = 1 / distance;

        for (float lerp = 0; lerp <= 1; lerp += lerp_steps) {
            cur_position = Vector2.Lerp(startPoint, endPoint, lerp);
            MarkPixelsToColour(cur_position);
        }
    }

    public void MarkPixelToChange(int x, int y) {
        int arrayPosition = (y * (int)drawSprite.rect.width) + x;

        if (arrayPosition > currentColors.Length || arrayPosition < 0) {
            return;
        }

        currentColors[arrayPosition] = Color.black;
    }

    public void ApplyCurrentColors() {
        drawTexture.SetPixels32(currentColors);
        drawTexture.Apply();
    }

    public void ResetTexture() {
        drawTexture.SetPixels(resetColorsArray);
        drawTexture.Apply();
    }

    public void OnDrag(PointerEventData eventData) {
        Vector2 localCursor = Vector2.zero;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, Input.mousePosition, eventData.pressEventCamera, out localCursor)) {
            return;
        }

        if (localCursor.x < rectTransform.rect.width &&
            localCursor.y < rectTransform.rect.height &&
            localCursor.x > 0 &&
            localCursor.y > 0) {
            float rectToPixelScale = drawImage.sprite.rect.width / rectTransform.rect.width;
            localCursor = new Vector2(localCursor.x * rectToPixelScale, localCursor.y * rectToPixelScale);
            Paint(localCursor);
            previousDragPosition = localCursor;
        } else {
            previousDragPosition = Vector2.zero;
        }


    }

    public void OnEndDrag(PointerEventData eventData) {
        previousDragPosition = Vector2.zero;
    }
}