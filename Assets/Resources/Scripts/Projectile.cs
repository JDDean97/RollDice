using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody rb;
    float speed = 15f;
    Vector3 prevPos;
    float lifeTime = 4;
    GameObject owner;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        prevPos = transform.position + transform.forward;
    }

    private void Update()
    {
        rayCheck();
        lifeTime -= 1 * Time.deltaTime;
        if(lifeTime<=0)
        {
            Destroy(gameObject);
        }
        prevPos = transform.position;
    }

    public void setSpeed(float s)
    {
        speed = s;
    }

    public void setOwner(GameObject g)
    {
        owner = g;
    }

    void rayCheck()
    {
        Ray r = new Ray(prevPos, transform.position);
        RaycastHit hit;
        //float dist = Vector3.Distance(prevPos, transform.position);
        float dist = 2;
        Debug.DrawRay(transform.position, (prevPos - transform.position) * dist, Color.red);
        if (Physics.Raycast(r, out hit,dist))
        {
            if(hit.transform.gameObject == owner)
            { return; }

            if(hit.transform.GetComponent<Player>())
            {
                hit.transform.GetComponentInParent<Player>().hurt(7);
            }
            else if (hit.transform.GetComponent<Enemy>())
            {
                Enemy e = hit.transform.GetComponent<Enemy>();
                e.Hurt();
            }
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision hit)
    {
        if (hit.transform.gameObject == owner)
        { return; }

        if (hit.transform.GetComponent<Player>())
        {
            hit.transform.GetComponentInParent<Player>().hurt(7);
        }
        else if (hit.transform.GetComponent<Enemy>())
        {
            Enemy e = hit.transform.GetComponent<Enemy>();
            e.Hurt();
        }
        Destroy(gameObject);
    }


}
