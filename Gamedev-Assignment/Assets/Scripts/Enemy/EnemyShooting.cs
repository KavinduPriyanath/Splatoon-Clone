using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting: MonoBehaviour{
    //public Color paintColor;
    
    /*
    public float minRadius = 0.05f;
    public float maxRadius = 0.2f;
    public float strength = 1;
    public float hardness = 1;
    [Space]*/
    ParticleSystem part;
    
    List<ParticleCollisionEvent> collisionEvents;

    private PlayerController player;

    void Start(){
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        //var pr = part.GetComponent<ParticleSystemRenderer>();
        //Color c = new Color(pr.material.color.r, pr.material.color.g, pr.material.color.b, .8f);
        //paintColor = c;

        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void OnParticleCollision(GameObject other) {
        
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        //Debug.Log(numCollisionEvents);
        
        if(other.gameObject.CompareTag("Player")){
            
            for  (int i = 0; i< numCollisionEvents; i++){
                Debug.Log("hitting");
                player.healthPoints = (player.healthPoints > 0f) ? player.healthPoints - 0.5f : 0f;
            }
        }
    }
}