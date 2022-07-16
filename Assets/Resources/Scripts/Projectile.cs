using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody rb;
    const float speed = 25f;
    Vector3 prevPos;
    float lifeTime = 4;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        prevPos = transform.position;
    }

    private void Update()
    {
        lifeTime -= 1 * Time.deltaTime;
        if(lifeTime<=0)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rayCheck();
        prevPos = transform.position;
    }

    void rayCheck()
    {
        Ray r = new Ray(prevPos, transform.position);
        RaycastHit hit;
        float dist = Vector3.Distance(prevPos, transform.position);
        if(Physics.Raycast(r, out hit,dist))
        {
            if(hit.transform.GetComponent<Player>())
            {
                hit.transform.GetComponent<Player>().hurt(7);
            }
            if (hit.transform.GetComponent<Enemy>())
            {
                Enemy e = hit.transform.GetComponent<Enemy>();
                e.Hurt();
            }
            Destroy(gameObject);
        }
    }
}
