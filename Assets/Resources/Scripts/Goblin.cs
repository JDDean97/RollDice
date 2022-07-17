using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy
{
    [SerializeField]
    GameObject sword;
    [SerializeField]
    GameObject crossbow;
    public bool useBow = false;
    float range = 5;
    // Start is called before the first frame update
    void Start()
    {
        base.onStart();
        if(Random.Range(0,2)>0)
        {
            useBow = true;
        }
        else
        {
            useBow = false;
        }
        anim.SetBool("Bow", useBow);
        if(useBow)
        { 
            range = 15;
            sword.SetActive(false);
        }
        else
        {
            crossbow.SetActive(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(health<1)
        { return; }
        move();
        attack(range);
    }

    public override void Die()
    {
        base.Die();
        if(useBow)
        {
            anim.SetTrigger("DeathBow");
        }
        else 
        {
            anim.SetTrigger("DeathSword");
        }
        Destroy(this.GetComponentInChildren<Collider>());
        Destroy(this.GetComponent<Rigidbody>());
        Destroy(this.GetComponent<Enemy>());
    }

    public void shoot() //called by animation event
    {
        GameObject bolt = Instantiate(Resources.Load<GameObject>("Prefabs/BoltEnemy"));
        bolt.transform.position = transform.Find("BoltSpawn").position;
        bolt.transform.rotation = transform.rotation;
        bolt.GetComponent<Projectile>().setOwner(gameObject);
        
    }
}
