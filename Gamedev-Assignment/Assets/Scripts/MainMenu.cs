using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private ParticleSystem blueParticles;

    [SerializeField] private ParticleSystem yellowParticles;
    [SerializeField] private ParticleSystem greenParticles;
    [SerializeField] private ParticleSystem pinkParticles;
    [SerializeField] private ParticleSystem orangeParticles;

    private bool yellow;
    private bool green;
    private bool pink;
    private bool orange;

    [SerializeField] private Animator yellowAnim;
    [SerializeField] private Animator greenAnim;
    [SerializeField] private Animator pinkAnim;
    [SerializeField] private Animator orangeAnim;
    
    private void Start()
    {
        blueParticles.Play();
    }

    private void Update()
    {
        if (!yellow && Time.time > 5f)
        {
            yellowAnim.SetBool("yellow", true);
            yellowParticles.Play();
            yellow = true;
        }
        
        if (!green && Time.time > 15f)
        {
            greenAnim.SetBool("green", true);
            greenParticles.Play();
            green = true;
        }
        
        if (!pink && Time.time > 30f)
        {
            pinkAnim.SetBool("pink", true);
            pinkParticles.Play();
            pink = true;
        }
        
        if (!orange && Time.time > 45f)
        {
            orangeAnim.SetBool("orange", true);
            orangeParticles.Play();
            orange = true;
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
