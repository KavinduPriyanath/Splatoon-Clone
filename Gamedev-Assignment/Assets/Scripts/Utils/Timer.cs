using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private TMP_Text countdownText;

    private float currentTime;
    private float initialTime = 300f; // 5 minutes in seconds

    private void Start()
    {
        currentTime = initialTime;
    }

    private void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);

            string timeFormatted = string.Format("{0}:{1:00}", minutes, seconds);
            countdownText.text = timeFormatted;
        }
        else
        {
            // Countdown timer has reached zero.
            countdownText.text = "0:00"; // You can add any desired behavior here.
        }
    }
}
