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
    private bool collectable;

    private GameObject currentLid;
    private GameObject currentHealth;

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

            if (collectable)
            {
                player.healthPoints += 15;
                pickupText.SetActive(false);
                Destroy(currentHealth);
                collectable = false;
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

        if (other.gameObject.CompareTag("Health"))
        {
            pickupText.SetActive(true);
            collectable = true;
            currentHealth = other.gameObject;
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
        
        if (other.gameObject.CompareTag("Health"))
        {
            pickupText.SetActive(false);
            collectable = false;
            currentHealth = null;
        }
    }
}
