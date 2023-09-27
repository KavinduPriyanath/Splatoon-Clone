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
    }
}
