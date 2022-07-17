using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    float range = 10;
    float _speed = 7f;
    const int cloneMax = 2;
    int cloneCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        base.onStart();
        health = 40;
        base.speed = _speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 1)
        { return; }
        move();
    }

    public override void Hurt(float _dmg = 20)
    {
        base.Hurt(_dmg);
    }

    public void setClone(int num)
    {
        cloneCounter = num;
    }


    public override void Die()
    {
        
        if(cloneCounter<cloneMax)
        {
            Vector3 offset = new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));
            GameObject baby = Instantiate(gameObject, transform.position + offset, Quaternion.identity);
            baby.transform.localScale = transform.localScale / 2;
            baby.GetComponent<Slime>().setClone(cloneCounter + 1);

            baby = Instantiate(gameObject, transform.position, Quaternion.identity);
            baby.transform.localScale = transform.localScale / 2;
            baby.GetComponent<Slime>().setClone(cloneCounter + 1);
        }

        base.Die();
        anim.SetTrigger("Die");
        GetComponentInChildren<Renderer>().material = Resources.Load<Material>("Materials/matSlimeDead");
        Destroy(this.GetComponentInChildren<Collider>());
        Destroy(this.GetComponent<Rigidbody>());
        Destroy(this.GetComponent<Enemy>());
    }
}
