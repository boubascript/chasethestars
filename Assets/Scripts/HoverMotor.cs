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

    public AudioSource engine;

    private bool manualMode;

    void Awake()
    {
        carRigidbody = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        manualMode = true;
    }

    void Update()
    {
        accelerating = Input.GetButton("Vertical"); 
        reversing = Input.GetButton("Reverse");

        amove = Camera.main.transform.localRotation;
        turnInput = !manualMode ? (amove[1]) * 10 : Input.GetAxis("Horizontal");
        v = amove[0]*-5;
       
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

            if(!engine.isPlaying){
                engine.Play();
            }
        }
        else if (reversing)
        {
            if(!engine.isPlaying){
                engine.Play();
            }
            carRigidbody.AddForce(transform.forward * -speed * .5f, ForceMode.Acceleration);
        }
        else
        {
            engine.Stop();
            burnerParticles.Stop();
        }
        float height = (hoverHeight - hit.distance) / hoverHeight;
        Vector3 force_down = Vector3.up* height* -50.0f;
        carRigidbody.AddForce(force_down, ForceMode.Acceleration);

        carRigidbody.AddForce(transform.forward * 1.5f, ForceMode.Impulse);
        carRigidbody.transform.Rotate(new Vector3(0f, !manualMode ? turnInput : smoothedTurn * turnSpeed, 0f));

       
    }

}