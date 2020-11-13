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

        if (Input.GetButtonDown("Jump"))
        {
            lightsOn = !lightsOn;
            headlight.enabled = lightsOn;
        }
        // fmove = Camera.main.transform.rotation.eulerAngles;
        // fmove = Vector3.Normalize(fmove);
        amove = Camera.main.transform.localRotation;
        // amove = Quaternion.Inverse(amove);
        // fmove.y = fmove.z;
        // (Camera.main.transform.rotation[1])*9.0f
        turnInput = (amove[1])*-10 ;
        v = amove[0]*-5;
        Debug.Log(turnInput);
        // v = Input.GetAxis("Verticle");
        // carRigidbody.transform. += Vector3.ProjectOnPlane(Camera.main.transform.up,Vector3.up)*0.8f;

        // MANUAL
        // turnInput = Input.GetAxis("Horizontal");
       
    }

    void FixedUpdate()
    {
        
        /*
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        Vector3 movement = v * camForward + turnInput * camRight;
        */
        
        // movement = transform.InverseTransformDirection(movement);
        // movement = Vector3.ProjectOnPlane(movement, groundNormal);
     
        // targetdirection.y = 0.0f;
        


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
        // carRigidbody.AddForce(targetdirection * speed);
        // forward=Camera.main.transform.TransformDirection(Vector3 (0,0,0));
        // forward.y = forward.x*10.0f;
        // forward.z = 0;
        // forward.x = 0;
        

        carRigidbody.transform.Rotate(new Vector3(0f, turnInput, 0f));

        // MANUAL
        // carRigidbody.transform.Rotate(new Vector3(0f, smoothedTurn * turnSpeed, 0f));

        // carRigidbody.transform.Rotate(new Vector3(0f, turnInput*-1, 0f));
        // carRigidbody.transform.Rotate();
        // new Vector3(0f, smoothedTurn * turnSpeed, 0f)
    }

}