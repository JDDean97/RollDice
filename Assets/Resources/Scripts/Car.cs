using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    /// <summary>
    /// use driftController.cs for reference
    /// </summary>
    Rigidbody rb;
    public float accelBase = 15;
    float accel = 0;  
    float torque = 0;
    public float topSpeed = 20;
    public float brakeStrength = 10;
    float gripX = 12;
    float gripZ = 3;
    List <GameObject> backTires = new List<GameObject>();
    float angDragG = 5; //angular drag on ground
    float angDragA = 0.05f; //angular drag in air
    Vector3 vel = Vector3.zero;
    Vector3 pvel = Vector3.zero;
    bool isRotating = false;
    float rotVel = 0.8f;
    public AnimationCurve slipLower; //slip curve static to full (x, input = speed)
    public AnimationCurve slipUpper; //slip curve static to full (y, output = slip ratio)
    float slipMod = 10;
    float slip;
    bool inSlip = false;
    float rotate = 0;
    public float rotateBase = 90;
    public float maxRotSpd = 4;
    public float minRotSpd = 1;
    bool isGrounded = true;
    float distToGround = 0;
    Bounds bounds;
    float dir = 0;
    float boost = 0;
    float boostPower = 0.002f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        distToGround = GetComponent<Collider>().bounds.extents.y;
        rb.centerOfMass = rb.centerOfMass - Vector3.up * 0.5f;
        accelBase = 20 + (30 * (PlayerPrefs.GetFloat("accel")*0.1f));
        topSpeed = 20 + (30 * (PlayerPrefs.GetFloat("speed")*0.1f));
        rotateBase = 60 + (90 * (PlayerPrefs.GetFloat("turn")*0.1f));
        brakeStrength = 20 + (40 * (PlayerPrefs.GetFloat("brake")*0.1f));
        boostPower = 0.001f + (0.002f * (PlayerPrefs.GetFloat("boost")*0.1f));
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, rb.velocity, Color.red);
        Debug.DrawRay(transform.position, transform.forward * 10, Color.green);
        Debug.DrawRay(transform.position, -transform.up * (distToGround + 0.5f), Color.blue);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        accel = accelBase;
        rotate = rotateBase;
        gripX = 12;
        gripZ = 3;
        rotVel = 0.8f;
        rb.angularDrag = angDragG;
        isGrounded = Physics.Raycast(transform.position,-transform.up,distToGround +0.5f);
        if(!isGrounded)
        {
            rotate = 0;
            accel = 0;
            gripX = 0;
            gripZ = 0;
            rb.angularDrag = angDragA;
        }
        accel = accel * Mathf.Cos(transform.eulerAngles.x * Mathf.Deg2Rad);
        accel = accel > 0f ? accel : 0f;

        isRotating = false;
        
        if (rb.velocity.magnitude < minRotSpd){        
            rotate = 0;        
        } else{
            rotate = rb.velocity.magnitude / maxRotSpd * rotate;
        }

        if (!inSlip){
            slip = this.slipLower.Evaluate(Mathf.Abs(pvel.x) / slipMod);
            if (slip == 1f) inSlip = true;
        } else{
            slip = this.slipUpper.Evaluate(Mathf.Abs(pvel.x) / slipMod);
            if (slip == 0f) inSlip = false;
        }

        if(Input.GetKey(KeyCode.Space))
        {
            slip = 1f;
        }


        rotate *= 1 - 0.3f * slip;
        rotVel *= 1 - slip;
        pvel = transform.InverseTransformDirection(rb.velocity);
        turn();
        throttle();
        
        vel = transform.InverseTransformDirection(rb.velocity);
               
        
        if(isRotating)
        {
            vel = vel *(1-rotVel)+ pvel * rotVel; //partial velocity transfer when turning
        }
        friction();
        vel += vel * boost;
        rb.velocity = transform.TransformDirection(vel);
        boost = 0;
    }

    public void setBoost()
    {
        boost = boostPower;
    }

    public bool getSlip()
    {
        return inSlip;
    }

    void throttle()
    {
        vel = transform.InverseTransformDirection(rb.velocity);
        torque = Input.GetAxis("Vertical");
        float brake = 0;        
        rb.velocity += (transform.forward * torque) * accel * Time.deltaTime;
        if(vel.z> 0 && torque < 0)
        {
            brake = brakeStrength;
            rb.velocity += -rb.velocity.normalized * brakeStrength * Time.deltaTime;
        }
        else if(vel.z<0 && torque>0)
        {
            brake = brakeStrength;
            rb.velocity += -rb.velocity.normalized * brakeStrength * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.Space)) //brake when user pulls handbrake
        {
            brake = brakeStrength * 0.2f;
            rb.velocity += -rb.velocity.normalized * brakeStrength * Time.deltaTime;
        }
    }

    void turn()
    {
        rotate = Mathf.Clamp(rotate, 0, rotateBase);
        float ang = Vector3.SignedAngle(rb.velocity, transform.forward, transform.up);        
        float tempDir = (Mathf.Abs(ang) > 105) ? -1 : 1;
        dir = Mathf.Lerp(dir, tempDir, 1 * Time.deltaTime);
        Vector3 rot = new Vector3(0,Input.GetAxis("Horizontal") * dir * rotate * Time.deltaTime,0);
        //Debug.Log("rotate: " + rotate);
        if (Input.GetAxis("Horizontal")!=0)
        {
            isRotating = true;
        }
        transform.rotation *= Quaternion.AngleAxis(rot.y, transform.up);
    }

    void friction() //call after turn
    {
        gripX = gripX * Mathf.Cos(transform.eulerAngles.z *Mathf.Deg2Rad);
        gripZ = gripZ * Mathf.Cos(transform.eulerAngles.x * Mathf.Deg2Rad);
        gripZ = gripZ > 0 ? gripZ : 0;
        gripX = gripX > 0 ? gripX : 0;
        //Debug.Log(gripX);

        float isRight = vel.x > 0 ? 1 : -1;
        vel.x -= isRight * gripX * Time.deltaTime;
        if (vel.x * isRight < 0) vel.x = 0;

        float isForward = vel.z > 0 ? 1 : -1;
        vel.z -= isForward * gripZ * Time.deltaTime;
        if (vel.z * isForward < 0) vel.z = 0;

        if (vel.z>topSpeed){
            vel.z = topSpeed;
        } else if (vel.z < -topSpeed){
            vel.z = -topSpeed;
        }
    }
}
