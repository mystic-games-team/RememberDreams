using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    struct PlayerInput
    {
        public float move_x;
        public bool jump;
        public bool attack;
    }


    public enum PlayerStates
    {
        IDLE = 1,
        RUN = 2,
        JUMPING = 3,
        AIR = 4,
        ATTACK=5,

        NONE = -1
    }


    public float velocity = 0.0f;
    public float jump_force = 0.0f;
    public PlayerStates player_state = PlayerStates.NONE; 

    private Rigidbody2D rigid_body;
    private Animator anim;
    private SpriteRenderer sprite_renderer;
    private PlayerInput player_input; // variables de input en una struct

    private float time_jump_start = 0.0F; // no he trobat cap timer, he trobat el mateix que SDL_GetTicks(), Time.realtimeSinceStartup

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
        Debug.Log((int)player_state);
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
            case PlayerStates.RUN:
                HoritzontalMovement();
                break;
            case PlayerStates.JUMPING:
                Jump();
                HoritzontalMovement();
                break;
            case PlayerStates.AIR:
                HoritzontalMovement();
                break;
            default:
                break;
        }
    }

    private void GetInput()
    {
        player_input.jump = Input.GetKey(KeyCode.Space);
        player_input.move_x = Input.GetAxis("Horizontal");
        player_input.attack = Input.GetKey(KeyCode.Z);
    }

    private void Jump()
    {
        rigid_body.AddForce(Vector2.up * jump_force, ForceMode2D.Impulse);
    }

    private void Attack()
    {
        if(player_input.attack)
        {
            player_state = PlayerStates.ATTACK;
        }
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
                CanJump();
                Attack();
                if (player_input.move_x != 0)
                    player_state = PlayerStates.RUN;
                break;
            case PlayerStates.RUN:
                CanJump();
                Attack();
                if (player_input.move_x == 0)
                    player_state = PlayerStates.IDLE;
                break;
            case PlayerStates.JUMPING:
                if (!player_input.jump || time_jump_start < Time.realtimeSinceStartup - 0.3F)
                {
                    player_state = PlayerStates.AIR;
                }
                break;
            case PlayerStates.AIR:
               // The logic is in DetectGround.cs
                break;
            case PlayerStates.ATTACK:
                
                break;
            default:
                break;
        }
    }

    private void CanJump()
    {
        if (player_input.jump)
        {
            player_state = PlayerStates.JUMPING;
            time_jump_start = Time.realtimeSinceStartup;
        }
    }

    private void AttackFinished()
    {
        player_state = PlayerStates.IDLE;
    }
}

