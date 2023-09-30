using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using Unity.Collections;
using Unity.Jobs;
using System.Threading.Tasks;
using UnityEngine.UI;

public class PaintedPercentage : MonoBehaviour
{

    //[SerializeField] private Paintable paintSurface;
    public bool calculate;

    public List<Paintable> paintSurfaces;

    private Dictionary<Paintable, float> paintSurfaceDictionaryPlayer = new Dictionary<Paintable, float>();
    private Dictionary<Paintable, float> paintSurfaceDictionaryEnemy = new Dictionary<Paintable, float>();

    [SerializeField] private TMP_Text percentagePlayerText;
    [SerializeField] private TMP_Text percentageEnemyText;
    
    [SerializeField] private int percentage;

    private float startTime;
    private bool runThread;

    [SerializeField] private Image playerTimerImage;
    [SerializeField] private Image enemyTimerImage;
    private float timer;
    private float fillDuration = 5.0f;
    private bool isFilling;
    
    [SerializeField] private TMP_Text playerPercentage;
    [SerializeField] private TMP_Text enemyPercentage;
    [SerializeField] private TMP_Text differenceText;
    private int playerPercet;
    private int enemyPercent;
    [SerializeField] private TMP_Text preMessage;
    [SerializeField] private TMP_Text finalMessage;
    [SerializeField] private GameObject winButtons;
    [SerializeField] private GameObject loseButtons;
    
    private void Start()
    {
        //Adding the list's surfaces into the dictionary
        foreach (Paintable paintSurface in paintSurfaces)
        {
            paintSurfaceDictionaryPlayer.Add(paintSurface, 0f);
            paintSurfaceDictionaryEnemy.Add(paintSurface, 0f);
        }

        startTime = Time.time;
        runThread = true;
        isFilling = true;
    }

    private void Update()
    {
        //await Task.Run(() => CalculatePercentage());
        if (Mathf.Abs(Time.time - startTime) > 5f)
        {
            CalculatePercentage();
            startTime = Time.time;
        }

        if (isFilling)
        {
            isFilling = false;
            StartCoroutine(FillImage(playerTimerImage));
            StartCoroutine(FillImage(enemyTimerImage));
        }

        if (GameManager.managerInstance.gameOver)
        {
            playerPercentage.text = percentagePlayerText.text;
            enemyPercentage.text = percentageEnemyText.text;

            if (playerPercet > enemyPercent)
            {
                finalMessage.text = "Congratulations!!!";
                preMessage.text = "You've Won";
                winButtons.SetActive(true);
                loseButtons.SetActive(false);
            }
            else
            {
                finalMessage.text = "Try Again";
                preMessage.text = "You've Lost";
                winButtons.SetActive(false);
                loseButtons.SetActive(true);
            }
            
            int difference = Mathf.Abs(playerPercet - enemyPercent);
            differenceText.text = difference + "%";
        }

    }

    private IEnumerator FillImage(Image image)
    {
        timer = 0.0f;
        while (timer < fillDuration)
        {
            float fillAmount = timer / fillDuration;
            image.fillAmount = fillAmount;
            timer += Time.deltaTime;
            yield return null;
        }

        isFilling = true;

    }

    private void CalculatePercentage()
    {
        foreach (Paintable paintSurface in paintSurfaces)
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
                int matchingPixelCountPlayer = 0;
                int matchingPixelCountEnemy = 0;

                foreach (Color pixelColor in pixels)
                {
                    if ((pixelColor.r >= 0.3f && pixelColor.r <= 0.4f) && (pixelColor.g >= 0.8f && pixelColor.g <= 0.9f) && (pixelColor.b >= 0.1f && pixelColor.b <= 0.2f))
                    {
                        matchingPixelCountEnemy++;
                    }
                    if ((pixelColor.r >= 0.9f && pixelColor.r <= 1f) && (pixelColor.g >= 0.2f && pixelColor.g <= 0.3f) && (pixelColor.b >= 0.4f && pixelColor.b <= 0.5f))
                    {
                        matchingPixelCountPlayer++;
                    }
                }

                float percentagePlayer = (float)matchingPixelCountPlayer / (float)(width * height) * 100f;
                paintSurfaceDictionaryPlayer[paintSurface] = percentagePlayer;
                //await Task.Delay(1000);
                //yield return new WaitForSeconds(2f);
                
                //for enemy painting
                float percentageEnemy = (float)matchingPixelCountEnemy / (float)(width * height) * 100f;
                paintSurfaceDictionaryEnemy[paintSurface] = percentageEnemy;
            }
        }

        
        float totalPlayer = 0f;
        float totalEnemy = 0f;
        foreach (float value in paintSurfaceDictionaryPlayer.Values)
        {
            totalPlayer += value;
        }
        
        foreach (float value in paintSurfaceDictionaryEnemy.Values)
        {
            totalEnemy += value;
        }

        float totalPercentagePlayer = totalPlayer / paintSurfaceDictionaryPlayer.Count;
        int finalPercentagePlayer = (int)Math.Ceiling(totalPercentagePlayer);
        playerPercet = finalPercentagePlayer;
        
        float totalPercentageEnemy = totalEnemy / paintSurfaceDictionaryEnemy.Count;
        int finalPercentageEnemy = (int)Math.Ceiling(totalPercentageEnemy);
        enemyPercent = finalPercentageEnemy;
        
        percentageEnemyText.text = finalPercentageEnemy + "%";
        percentagePlayerText.text = finalPercentagePlayer + "%";

    }
}
