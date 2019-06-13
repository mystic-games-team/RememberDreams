using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocity = 0.0f;

    private Rigidbody2D rigid_body;

    // Start is called before the first frame update
    void Start()
    {
            rigid_body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FixedUpdate()
    {
        Vector2 curVel = rigid_body.velocity;
        curVel.x = Input.GetAxis("Horizontal") * velocity;
        rigid_body.velocity = curVel;

        Debug.Log(rigid_body.velocity);
    }
}
