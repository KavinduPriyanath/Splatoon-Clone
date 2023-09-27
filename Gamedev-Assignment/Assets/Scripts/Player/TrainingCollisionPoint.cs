using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingCollisionPoint : MonoBehaviour
{
    [SerializeField] private Training trainScript;
    [SerializeField] private GameObject walkInstruction;
    [SerializeField] private GameObject jumpInstruction;
    [SerializeField] private GameObject gunIntroInstruction;
    [SerializeField] private GameObject gunCloseIntroduction;
    [SerializeField] private GameObject gunPickupIntroduction;
    [SerializeField] private GameObject searchAmmoIntroduction;
    [SerializeField] private GameObject ammoPickIntroduction;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Space Intro")
        {
            jumpInstruction.SetActive(true);
            walkInstruction.SetActive(false);
            trainScript.canJump = true;
        }

        if (other.gameObject.name == "Gun Intro")
        {
            gunIntroInstruction.SetActive(true);
        }

        if (other.gameObject.name == "Gun Pickup")
        {
            gunCloseIntroduction.SetActive(false);
            gunPickupIntroduction.SetActive(true);
            trainScript.gunPickup = true;
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.name == "Reset Point 1")
        {
            searchAmmoIntroduction.SetActive(false);
        }

        if (other.gameObject.name == "Ammo Pick")
        {
            ammoPickIntroduction.SetActive(true);
        }

        if (other.gameObject.name == "Ammo Clip")
        {
            trainScript.ammoPickup = true;
            ammoPickIntroduction.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Ammo Clip")
        {
            trainScript.ammoPickup = false;
        }
    }
}
