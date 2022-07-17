using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Image imgHealth;
    Image imgBoost;
    Image imgNeedle;
    float health = 100;
    float topSpeed;
    float boost = 2;
    const float maxBoost = 2;
    const float boostFillRate = 1.5f;
    const float rechargeDelay = 1.8f;
    float rechargeTimer;
    Animator anim;
    Rigidbody rb;
    Car car;
    public TrailRenderer[] skidders = new TrailRenderer[4];
    DJ dj;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        car = GetComponent<Car>();
        imgHealth = FindObjectOfType<Canvas>().transform.Find("speedometer/health").GetComponent<Image>();
        imgBoost = FindObjectOfType<Canvas>().transform.Find("speedometer/boost").GetComponent<Image>();
        imgNeedle = FindObjectOfType<Canvas>().transform.Find("speedometer/needle").GetComponent<Image>();
        dj = FindObjectOfType<DJ>();
        topSpeed = car.topSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale<=0)
        { return; }
        rechargeTimer -= 1 * Time.deltaTime;

        if(rechargeTimer<0)
        { boost += boostFillRate * Time.deltaTime; }

        boost = Mathf.Clamp(boost, 0, maxBoost);
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            car.setBoost();
            boost -= 1 *Time.deltaTime;
            rechargeTimer = rechargeDelay;
        }

        if(Input.GetMouseButtonDown(0))
        {
            shoot();
        }

        if (car.getSlip() > 0.7f && car.getGrounded())//if car is drifting then increase health
        {
            health += 5f * Time.deltaTime;
            health = Mathf.Clamp(health, 0, 100);
            foreach (TrailRenderer tr in skidders)
            {
                tr.emitting = true;
            }
            dj.playSound("skid");

        }
        else
        {
            foreach (TrailRenderer tr in skidders)
            {
                tr.emitting = false;
            }
            dj.stopSound("skid");
        }

        animate();
        uiUpdate();
        //Debug.Log("Health: " + health);

    }

    void uiUpdate()
    {
        imgHealth.fillAmount = health / 100;
        imgBoost.fillAmount = boost / 2;

        float num = Mathf.Clamp(rb.velocity.magnitude / car.topSpeed, 0, 1);
        imgNeedle.rectTransform.rotation = Quaternion.Euler(0, 0, -210 - (300*num));
    }

    public float getSpeed()
    {
        return rb.velocity.magnitude;
    }

    void animate()
    {
        anim.SetFloat("velX", -Input.GetAxis("Horizontal"));
        anim.SetFloat("velY", -Input.GetAxis("Vertical"));
        anim.SetFloat("speed", rb.velocity.magnitude * 3f);
    }

    public void hurt(float dmg)
    {
        health -= dmg;
        if(health<1)
        {
            die();
        }
    }

    void die()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Explosion"),transform.position,Quaternion.identity);
        FindObjectOfType<Director>().gameOver(false);
    }

    public void shoot() //called by animation event
    {
        float boltSpeed = 10;
        if(rb.velocity.magnitude>2)
        {
           boltSpeed = rb.velocity.magnitude * 2;
        }
        GameObject bolt = Instantiate(Resources.Load<GameObject>("Prefabs/Bolt"));
        bolt.transform.position = transform.Find("BoltSpawn1").position;
        bolt.transform.rotation = transform.rotation;
        bolt.GetComponent<Projectile>().setSpeed(boltSpeed);
        bolt.GetComponent<Projectile>().setOwner(gameObject);

        bolt = Instantiate(Resources.Load<GameObject>("Prefabs/Bolt"));
        bolt.transform.position = transform.Find("BoltSpawn2").position;
        bolt.transform.rotation = transform.rotation;
        bolt.GetComponent<Projectile>().setSpeed(boltSpeed);
        bolt.GetComponent<Projectile>().setOwner(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("RoomTrigger"))
        {
            int num = FindObjectOfType<Director>().getRoomCount();
            num = Mathf.Clamp(num, 0, 4);
            other.GetComponentInParent<Room>().createNeighbors(num);
        }
        else if(other.CompareTag("EWeapon"))
        {
            hurt(3);
        }
        else if(other.CompareTag("Trophy"))
        {
            FindObjectOfType<Director>().gameOver(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(rb.velocity.magnitude>10)
        {
            dj.playSound("crash");
            if(!collision.gameObject.CompareTag("Enemy"))
            {
                hurt(rb.velocity.magnitude*0.25f);
                Instantiate(Resources.Load<GameObject>("Prefabs/Spark"),collision.contacts[0].point,Quaternion.identity);
            }
        }
    }
}
