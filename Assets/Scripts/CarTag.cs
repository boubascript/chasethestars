using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTag : MonoBehaviour
{
    private float runtime = 0.0f;
    private float score = 0.0f;
    private float shieldTime = 0.0f;
    private float shieldMaxTime = 10.0f;
    private float bonusWeight = 5.0f;
    private int crashCount = 0, bonuses = 0, shieldHits = 0;
    private bool hasShield = false;
    private GameObject[] shields, obstacles;
    private GameObject hovercar;
    private Rigidbody hovercarBody;
    private LevelLayoutGenerator layoutGenerator;
    private HoverMotor motorScript;

    void Awake()
    {
        hovercar = GameObject.Find("HoverCar");
        hovercarBody = hovercar.GetComponent<Rigidbody>();
        layoutGenerator = GameObject.Find("LevelGenerator").GetComponent<LevelLayoutGenerator>();
        motorScript = GetComponent<HoverMotor>();
    }

    void Update()
    {
        runtime += Time.deltaTime;
        if(hasShield){
            shieldTime += Time.deltaTime;
            if (shieldTime >= shieldMaxTime){
                hasShield = false;
                shieldTime = 0.0f;
                GetComponent<HoverMotor>().headlight.enabled = false;
                foreach(GameObject s in shields)
                    s.SetActive(true);
                foreach(GameObject o in obstacles)
                    o.GetComponent<MeshCollider>().isTrigger = false;
                
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "obstacle"){
            score = runtime + bonusWeight * bonuses; 
            crashCount++;
            hovercar.transform.position =  layoutGenerator.spawnOrigin + new Vector3(0.0f, 40.0f, 840.0f);
            hovercarBody.velocity = crashCount > 5 ? Vector3.up * 10.0f : Vector3.zero;
            hovercarBody.angularVelocity = Vector3.zero;

            Debug.Log("SCORE: " + score + " Time: " + runtime + " seconds " + "Bonuses: " + bonuses + 
            " POV: " + (Camera.main.name == "Cam2" ? "First Person" : "Third Person") + 
            " Total CRASHES: " + crashCount);
            runtime = 0.0f;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "shield") {
            hasShield = true;
            other.gameObject.SetActive(false);
            obstacles = GameObject.FindGameObjectsWithTag("obstacle");
            foreach(GameObject o in obstacles)
                o.GetComponent<MeshCollider>().isTrigger = true;
            shields = GameObject.FindGameObjectsWithTag("shield");
            foreach(GameObject s in shields)
                s.SetActive(false);
            GetComponent<HoverMotor>().headlight.enabled = true;
        }

        if (other.gameObject.tag == "pointBoost") {
            other.gameObject.SetActive(false);
            bonuses++;
        }

        if (other.gameObject.tag == "obstacle") {
            shieldHits++;
        }

    }

}
