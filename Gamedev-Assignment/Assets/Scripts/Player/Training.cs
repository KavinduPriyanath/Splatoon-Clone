using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    private void Start()
    {
        instructions[0].SetActive(true);
        canMove = false;
        _isGrounded = true;
    }

    private void Update()
    {
        EnableCamera();
    }

    private void EnableCamera()
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
            StartCoroutine(HideObjects(instructions[12], instructions[12], 2f));
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
                instructions[12].SetActive(true);
                onShootMessage = false;
            }
            
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
            //refillMeter.value = paintCapacity;
            //fillImage.color = gradientImage.Evaluate(refillMeter.normalizedValue);
        } else if (Input.GetMouseButtonUp(0))
        {
            sprayPainter.Stop();
        }
    }
    
    private IEnumerator HideObjects(GameObject instruction, GameObject nextInstruction, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        instruction.SetActive(false);
        nextInstruction.SetActive(true);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }
}
