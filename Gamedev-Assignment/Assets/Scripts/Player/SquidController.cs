using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidController : MonoBehaviour
{
    public bool squidHit;

    private void Start()
    {
        squidHit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hittt");
        squidHit = true;
    }
}
