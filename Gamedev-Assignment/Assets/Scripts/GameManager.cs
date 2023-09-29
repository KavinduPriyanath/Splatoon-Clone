using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; set; }

    [SerializeField] private PlayerController player;
    [SerializeField] private CameraController cam;
    [SerializeField] private GameObject startMenu;

    public int enemyCount;
    [SerializeField] private List<Transform> enemySpawnPoints;
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private List<Transform> healthSpawnPoints;
    [SerializeField] private GameObject healthPrefab;
    
    private float currentTime;
    private float healthTime;
    
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Already has one");
            Destroy(gameObject);
        } else if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        startMenu.SetActive(true);
        player.enabled = false;
        cam.enabled = false;
        StartCoroutine(StartGame());
        enemyCount = 2;
        currentTime = Time.time;
        healthTime = Time.time;
    }

    private void Update()
    {
        if (enemyCount < 2 && (Mathf.Abs(currentTime - Time.time) > 120f))
        {
            int RandomNo = Random.Range(0, 5);
            Instantiate(enemyPrefab, enemySpawnPoints[RandomNo].position, Quaternion.identity);
            enemyCount++;
            currentTime = Time.time;
        }

        if (player.healthPoints < 50 && (Mathf.Abs(healthTime - Time.time) > 40f))
        {
            int randomInt = Random.Range(0, 2);
            Instantiate(healthPrefab, healthSpawnPoints[randomInt].position, quaternion.identity);
            healthTime = Time.time;
        }
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(7f);
        player.enabled = true;
        cam.enabled = true;
        startMenu.SetActive(false);
    }
}
