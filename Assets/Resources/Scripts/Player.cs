using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float health = 100;
    float boost = 2;
    const float maxBoost = 2;
    const float boostFillRate = 1.5f;
    const float rechargeDelay = 1.8f;
    float rechargeTimer;
    Animator anim;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
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
            GetComponent<Car>().setBoost(0.002f);
            boost -= 1 * Time.deltaTime;
            rechargeTimer = rechargeDelay;
        }

        animate();

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
