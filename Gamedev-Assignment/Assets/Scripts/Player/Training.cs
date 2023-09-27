using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    
    private void Start()
    {
        instructions[0].SetActive(true);
        canMove = false;
        _isGrounded = true;
        mainCamera = Camera.main;

        StartCoroutine(HidePipes());
    }

    private void Update()
    {
        if (instructions[0].activeSelf)
        {
            StartCoroutine(HideObjects(instructions[0], instructions[1], 2f));
        }
        
        if (instructions[1].activeSelf)
        {
            camController.enabled = true;
            StartCoroutine(HideObjects(instructions[1], instructions[2], 3f));
        }

        if (instructions[2].activeSelf)
        {
            canMove = true;
        }

        if (instructions[4].activeSelf)
        {
            StartCoroutine(HideObjects(instructions[4], instructions[5], 1f));
        }

        if (instructions[7].activeSelf)
        {
            StartCoroutine(HideObjects(instructions[7], instructions[8], 2f));
        }

        if (instructions[12].activeSelf)
        {
            StartCoroutine(HideObjects(instructions[12], instructions[13], 2f));
        }

        if (instructions[14].activeSelf)
        {
            StartCoroutine(HideObjects(instructions[14], squidPanel1, 2f));
        }

        if (Input.GetMouseButtonDown(0) && instructions[8].activeSelf)
        {
            instructions[8].SetActive(false);
            instructions[9].SetActive(true);
        }

        if (ammoPickup)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                instructions[11].SetActive(true);
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
                if (instructions[3].activeSelf)
                {
                    instructions[3].SetActive(false);
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
                gunPickup = false;
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
                onShootMessage = false;
            }
            
            if (paintCapacity <= 0)
            {
                instructions[13].SetActive(false);

                if (!outofPaint)
                {
                    instructions[14].SetActive(true);
                    outofPaint = true;
                    camController.enabled = false;
                    Cursor.lockState = CursorLockMode.None;
                }
                
                Debug.Log("Ran out of ink");
                return;
            }
            sprayPainter.Play();
        } else if (Input.GetMouseButton(0))
        {
            if (paintCapacity <= 0)
            {
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
        Cursor.lockState = CursorLockMode.Locked;
        squidModeUnlocked = true;
    }
}
