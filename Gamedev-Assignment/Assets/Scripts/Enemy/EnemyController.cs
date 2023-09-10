using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private ParticleSystem shootingEffect;
    private void Start()
    {
        shootingEffect.Play();
    }
}
