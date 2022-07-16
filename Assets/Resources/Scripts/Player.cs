using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Image imgHealth;
    Image imgBoost;
    float health = 100;
    float boost = 2;
    const float maxBoost = 2;
    const float boostFillRate = 1.5f;
    const float rechargeDelay = 1.8f;
    float rechargeTimer;
    Animator anim;
    Rigidbody rb;
    Car car;
    public TrailRenderer[] skidders = new TrailRenderer[4];
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        car = GetComponent<Car>();
        imgHealth = FindObjectOfType<Canvas>().transform.Find("speedometer/health").GetComponent<Image>();
        imgBoost = FindObjectOfType<Canvas>().transform.Find("speedometer/boost").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        rechargeTimer -= 1 * Time.deltaTime;

        if(rechargeTimer<0)
        { boost += boostFillRate * Time.deltaTime; }

        boost = Mathf.Clamp(boost, 0, maxBoost);
        if(Input.GetKey(KeyCode.LeftShift))
        {
            car.setBoost();
            boost -= 1 * Time.deltaTime;
            rechargeTimer = rechargeDelay;
        }

        if (car.getSlip() > 0.7f && car.getGrounded())//if car is drifting then increase health
        {
            health += 1 * Time.deltaTime;
            health = Mathf.Clamp(health, 0, 100);
            foreach (TrailRenderer tr in skidders)
            {
                tr.emitting = true;
            }

        }
        else
        {
            foreach (TrailRenderer tr in skidders)
            {
                tr.emitting = false;
            }
        }

        animate();
        uiUpdate();
        //Debug.Log("Health: " + health);

    }

    void uiUpdate()
    {
        imgHealth.fillAmount = health / 100;
        imgBoost.fillAmount = boost / 2;
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
    }
}
