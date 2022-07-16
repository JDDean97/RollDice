using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public Transform target;
    const float speed = 2.5f;
    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(11, 17, -8);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        follow();
    }

    void follow()
    {
        Vector3 lead = target.GetComponent<Rigidbody>().velocity.normalized * 5;
        float tSpeed = target.GetComponent<Rigidbody>().velocity.magnitude *0.8f; //speed of target
        Vector3 newPos = target.transform.position + lead + offset;
        transform.position = Vector3.Lerp(transform.position, newPos, (tSpeed + speed) * Time.deltaTime);
    }
}
