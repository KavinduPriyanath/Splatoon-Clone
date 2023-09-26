using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LidCollector : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    [SerializeField] private GameObject pickupText;

    [SerializeField] private TMP_Text lidCountText;
    
    private bool pickable;

    private GameObject currentLid;

    private void Start()
    {
        pickable = false;
        currentLid = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pickable)
            {
                player.lidCount += 1;
                Destroy(currentLid);
                pickupText.SetActive(false);
                pickable = false;
            }
        }

        lidCountText.text = player.lidCount.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            pickupText.SetActive(true);
            pickable = true;
            currentLid = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            pickupText.SetActive(false);
            pickable = false;
            currentLid = null;
        }
    }
}
