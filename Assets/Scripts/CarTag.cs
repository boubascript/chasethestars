using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTag : MonoBehaviour
{
    private int crashCount;
    private float runtime = 0.0f;

    void Update()
    {
        runtime += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("collided with " + collision.gameObject.name + " tag: " + collision.gameObject.tag);

        if(collision.gameObject.tag == "obstacle"){
            Debug.Log("Time: " + runtime + " seconds");
            runtime = 0.0f;
            GameObject hovercar = GameObject.Find("HoverCar");
            hovercar.transform.position = new Vector3(0.0f, 40.0f, 840.0f);
            Rigidbody rigidbody = hovercar.GetComponent<Rigidbody>();
            rigidbody.velocity = crashCount > 5 ? Vector3.up * 10.0f : Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;

            crashCount++;
            Debug.Log("POV: " + (Camera.main.name == "Cam2" ? "First Person" : "Third Person") + " Total CRASHES: " + crashCount);
        }
    }

}
