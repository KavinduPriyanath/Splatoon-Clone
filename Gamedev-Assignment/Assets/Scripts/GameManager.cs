using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private PlayerController player;
    [SerializeField] private CameraController cam;
    [SerializeField] private GameObject startMenu;
    
    private void Start()
    {
        startMenu.SetActive(true);
        player.enabled = false;
        cam.enabled = false;
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(7f);
        player.enabled = true;
        cam.enabled = true;
        startMenu.SetActive(false);
    }
}
