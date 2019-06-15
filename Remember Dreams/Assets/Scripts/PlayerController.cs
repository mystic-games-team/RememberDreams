using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerStates
    {
        IDLE = 1,
        WALK = 2,

        NONE = -1
    }


    public float velocity = 0.0f;
    public float jump_force = 0.0f;
    public PlayerStates player_state = PlayerStates.NONE;

    private Rigidbody2D rigid_body;
    private Animator anim;
    private SpriteRenderer sprite_renderer;

    // Start is called before the first frame update
    void Start()
    {
        rigid_body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetInteger("State", (int)player_state);
    }
    private void FixedUpdate()
    {
        Vector2 curVel = rigid_body.velocity;
        curVel.x = Input.GetAxis("Horizontal") * velocity;
        rigid_body.velocity = curVel;

        if (curVel.x > 0)
        {
            sprite_renderer.flipX = false;
        }

        if (curVel.x < 0)
        {
            sprite_renderer.flipX = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigid_body.AddForce(Vector2.up * jump_force, ForceMode2D.Impulse);

            Debug.Log("jaja hola");
        }
    }
}
