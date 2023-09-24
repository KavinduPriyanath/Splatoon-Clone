using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using TMPro;

public class PaintSurfaces : MonoBehaviour
{
    public bool calculate;

    [SerializeField] private List<Paintable> paintSurfaces;
    private Dictionary<Paintable, float> paintSurfaceDictionary = new Dictionary<Paintable, float>();

    [SerializeField] private TMP_Text percentageText;

    private void Start()
    {
        foreach (Paintable paintSurface in paintSurfaces)
        {
            paintSurfaceDictionary.Add(paintSurface, 0f);
        }
    }

    private async void Update()
    {
 
        
        await CalculatePercentageAsync();
        
    }

    private async Task CalculatePercentageAsync()
    {
        List<Task> tasks = new List<Task>();

        foreach (Paintable paintSurface in paintSurfaces)
        {
            if (paintSurface != null)
            {
                tasks.Add(Task.Run(() =>
                {
                    RenderTexture renderTexture = paintSurface.getMask();
                    int width = renderTexture.width;
                    int height = renderTexture.height;

                    RenderTexture.active = renderTexture;

                    // Read pixels directly from the RenderTexture
                    Texture2D tex = new Texture2D(width, height);
                    tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                    tex.Apply();
                    Color[] pixels = tex.GetPixels();

                    int matchingPixelCount = 0;

                    foreach (Color pixelColor in pixels)
                    {
                        // Your pixel color condition checks go here
                        // ...

                        // Example condition:
                        if ((pixelColor.r >= 0.3f && pixelColor.r <= 0.4f) && 
                            (pixelColor.g >= 0.8f && pixelColor.g <= 0.9f) && 
                            (pixelColor.b >= 0.1f && pixelColor.b <= 0.2f))
                        {
                            matchingPixelCount++;
                        }
                    }

                    float percentage = (float)matchingPixelCount / (float)(width * height) * 100f;
                    paintSurfaceDictionary[paintSurface] = percentage;
                }));
            }
        }

        await Task.WhenAll(tasks);

        float total = 0f;
        foreach (float value in paintSurfaceDictionary.Values)
        {
            total += value;
        }

        float totalPercentage = total / paintSurfaceDictionary.Count;
        int finalPercentage = (int)Math.Ceiling(totalPercentage);

        percentageText.text = finalPercentage + "%";
    }
}
