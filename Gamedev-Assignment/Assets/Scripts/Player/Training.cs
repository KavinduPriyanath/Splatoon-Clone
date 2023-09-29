using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Training : MonoBehaviour
{

    [SerializeField] private List<GameObject> instructions;
    [SerializeField] private CameraController camController;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private float movementSpeed;
    private bool canMove;

    public bool canJump;
    private bool _isGrounded;
    [SerializeField] private float jumpForce;

    public bool gunPickup;
    [SerializeField] private GameObject arms;

    public bool ammoPickup;
    private bool canShoot;
    
    [SerializeField] private int paintCapacity;
    [SerializeField] private ParticleSystem sprayPainter;
    private bool onShootMessage;
    public bool ammoPicked;

    [SerializeField] private GameObject squidPanel1;
    private bool outofPaint;

    private bool squidModeUnlocked;
    private Camera mainCamera;

    [SerializeField] private GameObject squidPrefab;
    [SerializeField] private GameObject squidPaintDetection;
    [SerializeField] private GameObject refillTank;
    [SerializeField] private List<GameObject> playerGraphics;
    public bool squidMode;
    [SerializeField] private GameObject targetCamPosition;
    
    [SerializeField] private LayerMask groundLayer;

    public float healthPoints;
    [SerializeField] private Slider refillMeter;
    [SerializeField] private Gradient gradientImage;
    [SerializeField] private Image fillImage;

    [SerializeField] private GameObject paintMarks;
    [SerializeField] private Animator secondDoor;
    private bool doorClosed;
    
    [SerializeField] private GameObject gunDummy;
    [SerializeField] private Animator gunHolder;

    public bool lidPickup;
    public int lidCount;
    [SerializeField] private GameObject lidIntroduction;
    
    [SerializeField] private GameObject crossHair;
    private GameObject currentHitObject;
    [SerializeField] private TMP_Text lidCountText;

    private bool callingCoroutine = false;
    private bool jumpCalled;
    private bool panelCalled;
    
    private void Start()
    {
        instructions[0].SetActive(true);
        canMove = false;
        _isGrounded = true;
        mainCamera = Camera.main;
        doorClosed = true;

        StartCoroutine(HidePipes());
    }

    private void Update()
    {
        if (instructions[0].activeSelf && !callingCoroutine)
        {
            callingCoroutine = true;
            StartCoroutine(HideObjects(instructions[0], instructions[1], 34f));
        }
        
        if (instructions[1].activeSelf && !callingCoroutine)
        {
            camController.enabled = true;
            callingCoroutine = true;
            StartCoroutine(HideObjects(instructions[1], instructions[2], 5f));
        }

        if (instructions[2].activeSelf)
        {
            canMove = true;
        }

        if (instructions[4].activeSelf && !callingCoroutine)
        {
            callingCoroutine = true;
            StartCoroutine(HideObjects(instructions[4], instructions[5], 4f));
        }

        if (instructions[7].activeSelf && !callingCoroutine)
        {
            callingCoroutine = true;
            StartCoroutine(HideObjects(instructions[7], instructions[8], 4f));
        }

        if (instructions[12].activeSelf && !callingCoroutine)
        {
            callingCoroutine = true;
            StartCoroutine(HideObjects(instructions[12], instructions[13], 4f));
        }

        if (instructions[14].activeSelf)
        {
            StartCoroutine(HideObjects(instructions[14], squidPanel1, 3f));
            Cursor.lockState = CursorLockMode.None;
        }

        if (squidPanel1.activeSelf && !panelCalled)
        {
            instructions[16].SetActive(true);
            panelCalled = true;
            StartCoroutine(OverrideMessages(instructions[16], "Lets learn bit about refilling"));
        }

        /*
        if (Input.GetMouseButtonDown(0) && instructions[8].activeSelf)
        {
            instructions[8].SetActive(false);
            instructions[9].SetActive(true);
        }*/
        
        if (instructions[8].activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                instructions[8].SetActive(false);
                instructions[9].SetActive(true);
                StartCoroutine(OverrideMessages(instructions[9], "Looks like it does not have any ammo in it. Lets search a bit"));
            }
        }

        if (instructions[9].activeSelf)
        {
            instructions[8].SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Q) && doorClosed)
        {
            secondDoor.SetBool("open", true);
            instructions[15].SetActive(true);
            doorClosed = false;
        }

        if (ammoPickup)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                instructions[10].SetActive(false);
                instructions[11].SetActive(true);
                StartCoroutine(OverrideMessages(instructions[11], "Let's try to shoot again. Press Left Mouse button again"));
                canShoot = true;
                onShootMessage = true;
                ammoPicked = true;
            }
        }

        if (canMove)
        {
            Move();
        }

        if (canJump)
        {
            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            {
                if (instructions[3].activeSelf && !jumpCalled)
                {
                    //instructions[3].SetActive(false);
                    jumpCalled = true;
                    StartCoroutine(OverrideMessages(instructions[3], "Looks like you are born for this"));
                }
                
                rb.AddForce(new Vector3(0,jumpForce,0), ForceMode.Impulse);
                _isGrounded = false;
            }
        }

        if (canShoot)
        {
            Shoot();
        }

        if (gunPickup)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                instructions[6].SetActive(false);
                arms.SetActive(true);
                instructions[7].SetActive(true);
                StartCoroutine(OverrideMessages(instructions[7], "Wow. This thing looks awesome."));
                gunDummy.SetActive(false);
                gunHolder.SetBool("down", true);
                gunPickup = false;
            }
        }

        if (lidPickup)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                lidCount = 2;
                lidIntroduction.SetActive(true);
                camController.enabled = false;
                this.enabled = false;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SquidModeToggle(squidMode);
        } else if (Input.GetKeyUp(KeyCode.Q) && squidMode == true)
        {
            SquidModeToggle(squidMode);
        }
        
        if (squidPrefab.activeSelf)
        {
            SquidPaintDetection();
        }
        
        if (Input.GetMouseButton(1))
        {
            crossHair.SetActive(true);
            LidPlacement();
        } else if (Input.GetMouseButtonUp(1))
        {
            crossHair.SetActive(false);
            if (currentHitObject != null)
            {
                currentHitObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }

        lidCountText.text = lidCount.ToString();
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal") * movementSpeed;
        float z = Input.GetAxis("Vertical") * movementSpeed;

        rb.velocity = (transform.forward * z) + (transform.right * x) + (transform.up * rb.velocity.y);
    }
    
    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (onShootMessage)
            {
                instructions[11].SetActive(false);
                instructions[12].SetActive(true);
                StartCoroutine(OverrideMessages(instructions[12], "wow. Its a paint gun so cool"));
                onShootMessage = false;
            }
            
            if (paintCapacity <= 0)
            {
                instructions[13].SetActive(false);

                if (!outofPaint)
                {
                    instructions[14].SetActive(true);
                    StartCoroutine(OverrideMessages(instructions[14], "Looks like you are out of paint"));
                    outofPaint = true;
                    camController.enabled = false;
                    //Cursor.lockState = CursorLockMode.None;
                }
                
                Debug.Log("Ran out of ink");
                return;
            }
            sprayPainter.Play();
        } else if (Input.GetMouseButton(0))
        {
            if (paintCapacity <= 0)
            {
                instructions[13].SetActive(false);

                if (!outofPaint)
                {
                    instructions[14].SetActive(true);
                    StartCoroutine(OverrideMessages(instructions[14], "Looks like you are out of paint"));
                    outofPaint = true;
                    camController.enabled = false;
                    Cursor.lockState = CursorLockMode.None;
                }
                
                Debug.Log("Ran out of ink");
                return;
            }
            paintCapacity -= 1;
            //refillMeter.value = paintCapacity;
            //fillImage.color = gradientImage.Evaluate(refillMeter.normalizedValue);
        } else if (Input.GetMouseButtonUp(0))
        {
            sprayPainter.Stop();
        }
    }
    
    private void LidPlacement()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Placement"))
            {
                MeshRenderer meshRenderer = hit.collider.GetComponent<MeshRenderer>();
                meshRenderer.enabled = true;
                currentHitObject = hit.collider.gameObject;

                if (Input.GetKeyDown(KeyCode.T) && lidCount > 0)
                {
                    Transform pipeObject = hit.transform.parent;
                    GameObject pipeSpray = pipeObject.Find("Spray Point").gameObject;
                    GameObject pipeCover = pipeObject.Find("Pipe Cover").gameObject;
                    //Todo - play a particle effect at pipe, and effect on lid UI
                    pipeSpray.SetActive(false);
                    pipeCover.SetActive(true);
                    currentHitObject.SetActive(false);
                    lidCount -= 1;
                }
            }

            if (currentHitObject != hit.collider.gameObject)
            {
                if (currentHitObject != null)
                {
                    currentHitObject.GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
    }
    
    
    private void SquidPaintDetection()
    {
        Ray ray = new Ray(squidPaintDetection.transform.position, Vector3.down);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            Paintable p = hit.collider.GetComponent<Paintable>();
            if (p != null)
            {
                
                Vector2 uv = hit.textureCoord;
                RenderTexture renderTexture = p.getMask();

                // Read the color from the RenderTexture using a temporary Texture2D
                Texture2D tempTex = new Texture2D(renderTexture.width, renderTexture.height);
                RenderTexture.active = renderTexture;
                tempTex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                tempTex.Apply();
                Color color = tempTex.GetPixelBilinear(uv.x, uv.y);
                RenderTexture.active = null;

                // Now you have the color of the surface at the hit point
                //Debug.Log("Color: " + color);

                float red = color.r;
                float green = color.g;
                float blue = color.b;

                if ((red >= 0.3f && red <= 0.4f) && (green >= 0.8f && green <= 0.9f) && (blue >= 0.1f && blue <= 0.2f))
                {
                    Debug.Log("Enemy Color");
                    healthPoints = (healthPoints > 0f) ? healthPoints - 0.25f :  0f;
                } else if ((red >= 0.9f && red <= 1f) && (green >= 0.2f && green <= 0.3f) && (blue >= 0.4f && blue <= 0.5f))
                {
                    Debug.Log("Player Color");
                    paintCapacity += 10;
                    refillMeter.value += 10;
                    fillImage.color = gradientImage.Evaluate(refillMeter.normalizedValue);
                    
                    if (paintCapacity >= 1000)
                    {
                        paintCapacity = 1000;
                        return;
                    }
                }

                // Clean up the temporary Texture2D
                Destroy(tempTex);
            }
        }
    }
    
    
    public void SquidModeToggle(bool squidStatus)
    {
        //Todo - Needs to adjust the camera when diving into squid mode
        if (squidStatus)
        {
            squidPrefab.SetActive(false);
            refillTank.SetActive(false);
            playerGraphics.ForEach(graphic => graphic.SetActive(true));
            camController.minAngle = -50f;
            camController.maxAngle = 50f;
            mainCamera.transform.localPosition = new Vector3(0f, 1.004f, 0f);
        }
        else
        {
            squidPrefab.SetActive(true);
            refillTank.SetActive(true);
            playerGraphics.ForEach(graphic => graphic.SetActive(false));
            camController.minAngle = 0f;
            camController.maxAngle = 5f;
            mainCamera.transform.position = Vector3.Lerp(Camera.main.transform.position,
                targetCamPosition.transform.position, 100 * Time.deltaTime);
            mainCamera.transform.position = targetCamPosition.transform.position;
            
        }

        squidMode = !squidStatus;
    }
    
    private IEnumerator HideObjects(GameObject instruction, GameObject nextInstruction, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        instruction.SetActive(false);
        nextInstruction.SetActive(true);

        if (nextInstruction.GetComponent<TMP_Text>() != null)
        {
            string currentText = nextInstruction.GetComponent<TMP_Text>().text;
            nextInstruction.GetComponent<TMP_Text>().text = "";

            for (int j = 0; j < currentText.Length; j++)
            {
 
                nextInstruction.GetComponent<TMP_Text>().text += currentText[j];
                yield return new WaitForSeconds(0.05f);
            }

            yield return new WaitForSeconds(1.5f);
            //nextInstruction.SetActive(false);
        }
        callingCoroutine = false;

        
        
    }

    private IEnumerator HidePipes()
    {
        yield return new WaitForSeconds(8f);
        paintMarks.SetActive(false);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }

    public void EnableCamera()
    {
        squidPanel1.SetActive(false);
        camController.enabled = true;
        this.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        squidModeUnlocked = true;
        instructions[16].SetActive(false);
        instructions[17].SetActive(true);
        StartCoroutine(OverrideMessages(instructions[17], "Less go squidsss"));
    }

    private IEnumerator TypingTexts(GameObject temp)
    {
        temp.SetActive(true);
        string currentText = temp.GetComponent<TMP_Text>().text;
        temp.GetComponent<TMP_Text>().text = "";

        for (int j = 0; j < currentText.Length; j++)
        {
 
            temp.GetComponent<TMP_Text>().text += currentText[j];
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1.5f);
        temp.SetActive(false);
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
