using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    Camera mainGameCamera;

    public float fadeSpeed = .2f;
    public float timer = 0f;

    private Texture2D blackTexture;
    public bool fade;
    private float textureTranspLevel;


    void Start()
    {
        // Make a black texture
        blackTexture = new Texture2D(1, 1);
        blackTexture.SetPixel(0, 0, new Color(0, 0, 0, 0));
        blackTexture.Apply();

    }


    // Put it in screen completely transparent
    void OnGUI()
    {
        Rect textureRectanngke = new Rect(0, 0, Screen.width, Screen.height);
        GUI.DrawTexture(textureRectanngke, blackTexture);
    }

    void Update()
    {

        // If fade is not triggured start unfading the screen
        if (!fade)
        {
            if (textureTranspLevel > 0)
            {
                textureTranspLevel -= Time.deltaTime * fadeSpeed;
                if (textureTranspLevel < 0) { textureTranspLevel = 0f; }
                blackTexture.SetPixel(0, 0, new Color(0, 0, 0, textureTranspLevel));
                blackTexture.Apply();
                return;
            }

        }

        // If fade is triggured start fading the screen
        if (fade)
        {
            timer += Time.deltaTime;
            if (timer >= 1/fadeSpeed)
            {
                fade = false;
                timer = 0f;
                return;  
            }
            
            if (textureTranspLevel < 1)
            {
                textureTranspLevel += Time.deltaTime * fadeSpeed;
                if (textureTranspLevel > 1) { textureTranspLevel = 1f; }
                blackTexture.SetPixel(0, 0, new Color(0, 0, 0, textureTranspLevel));
                blackTexture.Apply();
            }
        }
    }

    // Method to fade in out time
    public void CameraFadeOut()
    {

    }

    // Method to fade in over time
    public void CameraFade()
    {
        fade = true;
        timer = 0f;
    }

    public float GetFadeSpeed()
    {
        return fadeSpeed;
    }
}
