using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    [SerializeField] private float movementSpeed;

    private Rigidbody _rb;

    [SerializeField] private ParticleSystem sprayPainter;

    private Camera cam;

    [SerializeField] private GameObject squidPrefab;
    [SerializeField] private List<GameObject> playerGraphics;
    
    public bool squidMode;

    [SerializeField] private CameraController camController;

    [SerializeField] private int paintCapacity;
    [SerializeField] private float jumpForce;
    private bool _isGrounded;

    [SerializeField] private GameObject paintDetectionPoint;
    [SerializeField] private LayerMask groundLayer;

    public float healthPoints;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        squidMode = false;
        _isGrounded = true;
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal") * movementSpeed;
        float z = Input.GetAxis("Vertical") * movementSpeed;

        _rb.velocity = (transform.forward * z) + (transform.right * x) + (transform.up * _rb.velocity.y);
        
        Shoot();

        /*
        if (Input.GetMouseButtonDown(1))
        {
            ColorIdentifier();
        }*/

        if (Input.GetKeyDown(KeyCode.Z))
        {
            SquidModeToggle(squidMode);
        }

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rb.AddForce(new Vector3(0,jumpForce,0), ForceMode.Impulse);
            _isGrounded = false;
        }
        
        PaintDetection();
    }

    /*
    private void ColorIdentifier()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
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
                } else if ((red >= 0.9f && red <= 1f) && (green >= 0.2f && green <= 0.3f) && (blue >= 0.4f && blue <= 0.5f))
                {
                    Debug.Log("Player Color");
                }

                // Clean up the temporary Texture2D
                Destroy(tempTex);
            }
        }
    }*/

    private void PaintDetection()
    {
        Ray ray = new Ray(paintDetectionPoint.transform.position, Vector3.down);
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
                }

                // Clean up the temporary Texture2D
                Destroy(tempTex);
            }
        }
    }

    private void SquidModeToggle(bool squidStatus)
    {
        //Todo - Needs to adjust the camera when diving into squid mode
        if (squidStatus)
        {
            squidPrefab.SetActive(false);
            playerGraphics.ForEach(graphic => graphic.SetActive(true));
            camController.minAngle = -90f;
            camController.maxAngle = 90f;
        }
        else
        {
            squidPrefab.SetActive(true);
            playerGraphics.ForEach(graphic => graphic.SetActive(false));
            camController.minAngle = 0f;
            camController.maxAngle = 20f;
        }

        squidMode = !squidStatus;
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (paintCapacity <= 0)
            {
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
        } else if (Input.GetMouseButtonUp(0))
        {
            sprayPainter.Stop();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }
}
