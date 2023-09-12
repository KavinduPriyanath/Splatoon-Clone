using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintedPercentage : MonoBehaviour
{

    [SerializeField] private Paintable paintSurface;
    public bool calculate;
    
    private void Update()
    {
        if (calculate)
        {
            CalculatePercentage();
            calculate = false;
        }
    }

    private void CalculatePercentage()
    {
        if (paintSurface != null)
        {
            RenderTexture renderTexture = paintSurface.getMask();

            int width = renderTexture.width;
            int height = renderTexture.height;

            RenderTexture.active = renderTexture;
            Texture2D tex = new Texture2D(width, height);
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();
            RenderTexture.active = null;

            Color[] pixels = tex.GetPixels();
            int matchingPixelCount = 0;

            foreach (Color pixelColor in pixels)
            {
                if ((pixelColor.r >= 0.3f && pixelColor.r <= 0.4f) && (pixelColor.g >= 0.8f && pixelColor.g <= 0.9f) && (pixelColor.b >= 0.1f && pixelColor.b <= 0.2f))
                {
                    matchingPixelCount++;
                }
                
                if ((pixelColor.r >= 0.9f && pixelColor.r <= 1f) && (pixelColor.g >= 0.2f && pixelColor.g <= 0.3f) && (pixelColor.b >= 0.4f && pixelColor.b <= 0.5f))
                {
                    matchingPixelCount++;
                }
            }

            float percentage = (float)matchingPixelCount / (float)(width * height) * 100f;
            Debug.Log("Percentage is " + percentage);
        }
    }
}
