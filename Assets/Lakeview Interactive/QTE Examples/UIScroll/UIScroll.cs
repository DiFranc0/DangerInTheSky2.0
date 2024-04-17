using UnityEngine;
using UnityEngine.UI;

public class UIScroll : MonoBehaviour
{
    private RawImage rImage;

    public Vector2 scrollSpeed = new Vector2(0.2f,0.2f);

    private  int IMAGE_WIDTH; 
    private int IMAGE_HEIGHT;

    public float imageScale = 2;

    [ContextMenu("ResetImage")]
    void Start()
    {
        rImage = GetComponent<RawImage>();
        IMAGE_WIDTH = rImage.mainTexture.width;
        IMAGE_HEIGHT = rImage.mainTexture.height;
        var rect = rImage.uvRect;
        rect.width = Screen.width / (IMAGE_WIDTH * imageScale);
        rect.height = Screen.height / (IMAGE_HEIGHT * imageScale);
        rImage.uvRect = rect;
    }

    void Update()
    {
        var rect = rImage.uvRect;
        rect.width = Screen.width / (IMAGE_WIDTH * imageScale);
        rect.height = Screen.height / (IMAGE_HEIGHT * imageScale);

        Vector2 textureOffset = scrollSpeed * Time.deltaTime;

        rect.x += textureOffset.x;
        rect.y += textureOffset.y;
        rImage.uvRect = rect;
    }
}
