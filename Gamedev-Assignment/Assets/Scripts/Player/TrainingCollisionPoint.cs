using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingCollisionPoint : MonoBehaviour
{
    [SerializeField] private CameraController camScript;
    [SerializeField] private Training trainScript;
    [SerializeField] private GameObject walkInstruction;
    [SerializeField] private GameObject jumpInstruction;
    [SerializeField] private GameObject gunIntroInstruction;
    [SerializeField] private GameObject gunCloseIntroduction;
    [SerializeField] private GameObject gunPickupIntroduction;
    [SerializeField] private GameObject searchAmmoIntroduction;
    [SerializeField] private GameObject ammoPickIntroduction;
    [SerializeField] private GameObject enemyPaintIntroduction;
    [SerializeField] private GameObject enemyPaintSolutionIntroduction;
    [SerializeField] private GameObject lidGlanceIntroduction;
    [SerializeField] private GameObject canisterMessage;
    
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject light;

    [SerializeField] private GameObject world;
    [SerializeField] private Material newMat;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject CollectableUI;


    
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
            door.SetActive(true);
            light.SetActive(true);
            world.GetComponent<MeshRenderer>().material = newMat;
            healthBar.SetActive(true);
            CollectableUI.SetActive(true);
        }

        if (other.gameObject.name == "Ammo Clip")
        {
            trainScript.ammoPickup = true;
            ammoPickIntroduction.SetActive(false);
            if (trainScript.ammoPicked == true)
            {
                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.name == "Enemy Paint")
        {
            enemyPaintIntroduction.SetActive(true);
            canisterMessage.SetActive(false);
            StartCoroutine(HideText(enemyPaintIntroduction));
        }

        if (other.gameObject.name == "Enemy Paint Solution")
        {
            enemyPaintSolutionIntroduction.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            camScript.enabled = false;
            trainScript.enabled = false;
            Destroy(other.gameObject);
        }

        if (other.gameObject.name == "Lid Glance")
        {
            lidGlanceIntroduction.SetActive(true);
        }

        if (other.gameObject.name == "Lid Pickup")
        {
            trainScript.lidPickup = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Ammo Clip")
        {
            trainScript.ammoPickup = false;
        }
    }

    private IEnumerator HideText(GameObject textObject)
    {
        yield return new WaitForSeconds(2f);
        textObject.SetActive(false);
    }
    
    public void EnableAll()
    {
        camScript.enabled = true;
        trainScript.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
