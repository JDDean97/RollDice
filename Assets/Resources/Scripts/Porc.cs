﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porc : Enemy
{
    float range = 10;
    float speed = 8f;
    // Start is called before the first frame update
    void Start()
    {
        base.onStart();
        health = 200;
        base.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 1)
        { return; }
        move();
        attack(range);
    }

    public override void Hurt(float _dmg = 20)
    {
        base.Hurt(_dmg);
    }

    public override void Die()
    {
        base.Die();
        anim.SetTrigger("Die");
        Destroy(this.GetComponentInChildren<Collider>());
        Destroy(this.GetComponent<Rigidbody>());
        Destroy(this.GetComponent<Enemy>());
    }
}
