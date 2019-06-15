using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    struct PlayerInput
    {
        public float move_x;
        public bool jump;
    }


    public enum PlayerStates
    {
        IDLE = 1,
        WALK = 2,
        JUMPING = 3,
        AIR = 4,

        NONE = -1
    }


    public float velocity = 0.0f;
    public float jump_force = 0.0f;
    public PlayerStates player_state = PlayerStates.NONE;

    private Rigidbody2D rigid_body;
    private Animator anim;
    private SpriteRenderer sprite_renderer;
    private PlayerInput player_input;


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

        if (rigid_body.velocity.x > 0)
        {
            sprite_renderer.flipX = false;
        }
        else if (rigid_body.velocity.x < 0)
        {
            sprite_renderer.flipX = true;
        }
    }
    private void FixedUpdate()
    {
        GetInput();
        ChangeState();
        PerformActions();

    }

    private void PerformActions()
    {
        switch (player_state)
        {
            case PlayerStates.IDLE:
                break;
            case PlayerStates.WALK:
                HoritzontalMovement();
                break;
            case PlayerStates.JUMPING:
                Jump();
                break;
            default:
                break;
        }
    }

    private void GetInput()
    {
        player_input.jump = Input.GetKeyDown(KeyCode.Space);
        player_input.move_x = Input.GetAxis("Horizontal");
    }

    private void Jump()
    {
        rigid_body.AddForce(Vector2.up * jump_force, ForceMode2D.Impulse);
    }

    private void HoritzontalMovement()
    {
        Vector2 curVel = rigid_body.velocity;
        curVel.x = player_input.move_x * velocity;
        rigid_body.velocity = curVel;
    }


    private void ChangeState()
    {
        switch (player_state)
        {
            case PlayerStates.IDLE:
                if (player_input.jump)
                    player_state = PlayerStates.JUMPING;
                if (player_input.move_x != 0)
                    player_state = PlayerStates.WALK;
                break;
            case PlayerStates.WALK:
                if (player_input.jump)
                    player_state = PlayerStates.JUMPING;
                if (player_input.move_x == 0)
                    player_state = PlayerStates.IDLE;
                break;
            default:
                break;
        }
    }
}
