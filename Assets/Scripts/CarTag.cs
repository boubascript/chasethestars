using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTag : MonoBehaviour
{
    private float runtime = 0.0f;
    private float score = 0.0f;
    private float shieldTime = 0.0f;
    private float shieldMaxTime = 30.0f;
    private float bonusWeight = 5.0f;
    private int crashCount = 0, bonuses = 0, shieldHits = 0;
    private bool hasShield = false;

    private GameObject hovercar;
    private Rigidbody hovercarBody;
    private GameObject statsText;
    private GameObject[] shields, obstacles;
    private LevelLayoutGenerator layoutGenerator;
    private HoverMotor motorScript;

    public AudioSource power;
    public AudioSource crash;

    void Awake()
    {
        hovercar = GameObject.Find("HoverCar");
        hovercarBody = hovercar.GetComponent<Rigidbody>();
        layoutGenerator = GameObject.Find("LevelGenerator").GetComponent<LevelLayoutGenerator>();
        motorScript = GetComponent<HoverMotor>();
        statsText = GameObject.Find("StatsText");

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

        statsText.GetComponent<TextMesh>().text = "SCORE: " + score + " Bonuses: " + bonuses + " Time: " + runtime + " seconds " +
            "\nPOV: " + (Camera.main.name == "Cam2" ? "First Person" : "Third Person") + 
            " Total Lives: " + (3 - crashCount);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "obstacle"){
            crash.Play();
            score = runtime + bonusWeight * bonuses; 
            crashCount++;
            if(crashCount >= 3){
                hovercar.transform.position = layoutGenerator.spawnOrigin + new Vector3(0.0f, 40.0f, 840.0f);
                hovercarBody.velocity = Vector3.zero;
                hovercarBody.angularVelocity = Vector3.zero;
                crashCount = 0;
            }

            Debug.Log("SCORE: " + score + " Time: " + runtime + " seconds " + "Bonuses: " + bonuses + 
            " POV: " + (Camera.main.name == "Cam2" ? "First Person" : "Third Person") + 
            " Total CRASHES: " + crashCount);

            statsText.GetComponent<TextMesh>().text = "SCORE: " + score + " Time: " + runtime + " seconds " + "Bonuses: " + bonuses + 
            "\nPOV: " + (Camera.main.name == "Cam2" ? "First Person" : "Third Person") + 
            " Total Lives: " + (3 - crashCount);

            statsText.SetActive(true);
            runtime = 0.0f;
        }
    }

    private void OnTriggerEnter(Collider other) {
        power.Play();
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
