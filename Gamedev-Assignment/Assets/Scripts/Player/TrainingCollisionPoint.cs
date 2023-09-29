using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    [SerializeField] private GameObject onJumpDialogue;
    
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject light;

    [SerializeField] private GameObject world;
    [SerializeField] private Material newMat;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject CollectableUI;

    [SerializeField] private GameObject beforeLidCloseMessage;
    [SerializeField] private GameObject lidClosingMessage;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Space Intro")
        {
            walkInstruction.SetActive(false);
            StartCoroutine(StartTyping(jumpInstruction));
            trainScript.canJump = true;
        }

        if (other.gameObject.name == "Gun Intro")
        {
            jumpInstruction.SetActive(false);
            onJumpDialogue.SetActive(false);
            //gunIntroInstruction.SetActive(true);
            StartCoroutine(StartTyping(gunIntroInstruction));
        }

        if (other.gameObject.name == "Gun Pickup")
        {

            gunCloseIntroduction.SetActive(false);
            //gunPickupIntroduction.SetActive(true);
            StartCoroutine(StartTyping(gunPickupIntroduction));
            trainScript.gunPickup = true;
            other.gameObject.SetActive(false);
            
        }

        if (other.gameObject.name == "Reset Point 1")
        {
            searchAmmoIntroduction.SetActive(true);
            //StartCoroutine(StartTyping(searchAmmoIntroduction));
            StartCoroutine(OverrideMessages(searchAmmoIntroduction, "Wow. Sky looks amazing"));
        }

        if (other.gameObject.name == "Ammo Pick")
        {
            ammoPickIntroduction.SetActive(true);
            searchAmmoIntroduction.SetActive(false);
            door.SetActive(true);
            light.SetActive(true);
            world.GetComponent<MeshRenderer>().material = newMat;
            healthBar.SetActive(true);
            CollectableUI.SetActive(true);
            Destroy(other.gameObject);
        }

        if (other.gameObject.name == "Ammo Clip")
        {
            trainScript.ammoPickup = true;
            //ammoPickIntroduction.SetActive(false);
            
        }

        if (other.gameObject.name == "Enemy Paint")
        {
            enemyPaintIntroduction.SetActive(true);
            StartCoroutine(OverrideMessages(enemyPaintIntroduction, "There's something on floor"));
            canisterMessage.SetActive(false);
            //StartCoroutine(HideText(enemyPaintIntroduction));
        }

        if (other.gameObject.name == "Enemy Paint Solution")
        {
            StartCoroutine(OverrideMessages(enemyPaintIntroduction, "Learn this too"));
            enemyPaintSolutionIntroduction.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            camScript.enabled = false;
            trainScript.enabled = false;
            Destroy(other.gameObject);
        }

        if (other.gameObject.name == "Lid Glance")
        {
            enemyPaintIntroduction.SetActive(false);
            lidGlanceIntroduction.SetActive(true);
            StartCoroutine(OverrideMessages(lidGlanceIntroduction, "Something on floor, Pick it up"));
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
    
    public void LidClosingMessage()
    {
        beforeLidCloseMessage.SetActive(false);
        lidClosingMessage.SetActive(true);
        StartCoroutine(OverrideMessages(lidGlanceIntroduction, "Now close the pipes and proceed"));
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

    private IEnumerator StartTyping(GameObject instrucion)
    {
        instrucion.SetActive(true);
        string currentText = instrucion.GetComponent<TMP_Text>().text;
        instrucion.GetComponent<TMP_Text>().text = "";

        for (int j = 0; j < currentText.Length; j++)
        {
 
            instrucion.GetComponent<TMP_Text>().text += currentText[j];
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1.5f);
    }
    
    private IEnumerator OverrideMessages(GameObject temp, string message)
    {
        temp.SetActive(true);
        temp.GetComponent<TMP_Text>().text = "";
        string currentText = message;

        for (int j = 0; j < currentText.Length; j++)
        {
 
            temp.GetComponent<TMP_Text>().text += currentText[j];
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1.5f);
    }

    
}
