using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocity = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float axis = Input.GetAxis("Horizontal");
        
        if (axis < 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            transform.position = new Vector3(transform.position.x - velocity * Time.deltaTime, transform.position.y, transform.position.z);
        }
        else if (axis > 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            transform.position = new Vector3(transform.position.x + velocity * Time.deltaTime, transform.position.y, transform.position.z);
        }
    }
}
