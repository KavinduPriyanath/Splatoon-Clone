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

    [SerializeField] private Animator blueAnim;
    [SerializeField] private Animator yellowAnim;
    [SerializeField] private Animator greenAnim;
    [SerializeField] private Animator pinkAnim;
    [SerializeField] private Animator orangeAnim;

    private int currentAnimation = 0;
    
    private void Start()
    {
        blueParticles.Play();
    }

    private void Update()
    {
        //Checks whether the animation has completed playing, and if so stop the related particle system 
        if (currentAnimation == 0 && blueAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            //blueAnim.gameObject.SetActive(false);
            blueParticles.Stop();
            currentAnimation++;
        }
        
        if (currentAnimation == 1 && yellowAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            //blueAnim.gameObject.SetActive(false);
            yellowParticles.Stop();
            currentAnimation++;
        }
        
        if (currentAnimation == 2 && greenAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            //blueAnim.gameObject.SetActive(false);
            greenParticles.Stop();
            currentAnimation++;
        }
        
        if (currentAnimation == 3 && pinkAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            //blueAnim.gameObject.SetActive(false);
            pinkParticles.Stop();
            currentAnimation++;
        }
        
        if (currentAnimation == 4 && orangeAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            //blueAnim.gameObject.SetActive(false);
            orangeParticles.Stop();
            currentAnimation++;
        }
        
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
