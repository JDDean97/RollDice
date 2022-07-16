using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    protected float health = 100;
    protected NavMeshAgent nav;
    protected Rigidbody rb;
    protected Player player;
    protected const float speed = 10f;
    protected const float dmg = 50; //dmg taken when hurt
    protected Animator anim;
    // Start is called before the first frame update
    protected void onStart()
    {
        player = FindObjectOfType<Player>();
        nav = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    protected virtual void attack(float _Range)
    {
        if (Vector3.Distance(transform.position, player.transform.position) < _Range)
        {
            anim.SetTrigger("Attack");
        }
    }

    protected virtual void move()
    {
        nav.destination = player.transform.position;
        transform.LookAt(nav.steeringTarget);
        Vector3 spot = nav.steeringTarget;
        spot.y = transform.position.y;
        Vector3 dir = spot - transform.position;
        if(isState("RunBow")||isState("RunSword")||isState("Walk"))
        {
            rb.velocity = dir.normalized * speed;          
        }
        else if(isState("Charge"))
        {
            rb.velocity = dir.normalized * speed * 3;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        
    }

    protected bool isState(string name)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(name)||anim.GetNextAnimatorStateInfo(0).IsName(name))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void Hurt(float _dmg = dmg)
    {
        health -= _dmg;
        if(health<1)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        
    }

    protected void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PWeapon"))
        {
            if (other.GetComponentInParent<Rigidbody>().velocity.magnitude > 0.1f)
            {
                Hurt(dmg);
            }
        }
    }
}
