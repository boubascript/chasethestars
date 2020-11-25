using UnityEngine;
using System.Collections;

public class HoverMotor : MonoBehaviour
{

    public Transform cameras= Camera.main.transform;
    public float speed = 90f;
    public float turnSpeed = 10f;
    public float smoothing = .5f;
    public float hoverForce = 65f;
    public float hoverHeight = 3.5f;
    private float v;
    private Vector3 fmove;
    private Quaternion amove;
    public ParticleSystem burnerParticles;
    public Light headlight;
    private float powerInput;
    private float turnInput;
    private Rigidbody carRigidbody;
    private CharacterController controller;
    private bool accelerating = false;
    private bool reversing = false;
    private bool lightsOn = false;

    void Awake()
    {
        carRigidbody = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetButton("Vertical"))
        {
            accelerating = true;
        }
        else
        {
            accelerating = false;
            if (Input.GetButton("Reverse"))
            {
                reversing = true;
            }
            else
            {
                reversing = false;
            }
        }

        

        // VR Yaw Turn Control

        amove = Camera.main.transform.localRotation;
        turnInput = (amove[1])*10 ;
        v = amove[0]*-5;
        // turnInput = Input.GetAxis("Horizontal");
       
    }

    void FixedUpdate()
    {
        
       


        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, hoverHeight))
        {
            float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
            carRigidbody.AddForce(appliedHoverForce, ForceMode.Acceleration);
        }
        float tempInput = 0f;

        float smoothedTurn = Mathf.Lerp(turnInput, tempInput, smoothing);//revert to this later for turning
        //float turnagle = Mathf.Atan2(movement.x, movement.z);
        if (accelerating)
        {
            burnerParticles.Play();
            carRigidbody.AddForce(transform.forward * speed, ForceMode.Acceleration);
            //controller.Move(targetdirection * Time.deltaTime);
            reversing = false;
        }
        else if (reversing)
        {
            carRigidbody.AddForce(transform.forward * -speed * .5f, ForceMode.Acceleration);
        }
        else
        {
            burnerParticles.Stop();
        }
        float height = (hoverHeight - hit.distance) / hoverHeight;
        Vector3 force_down = Vector3.up* height* -50.0f;
        carRigidbody.AddForce(force_down, ForceMode.Acceleration);

        carRigidbody.AddForce(transform.forward * 1.5f, ForceMode.Impulse);
        carRigidbody.transform.Rotate(new Vector3(0f, turnInput, 0f));

       
    }

}