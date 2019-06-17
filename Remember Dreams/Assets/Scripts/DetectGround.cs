using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectGround : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground" && (GameObject.Find("Player").GetComponent<PlayerController>().player_state == PlayerController.PlayerStates.AIR || GameObject.Find("Player").GetComponent<PlayerController>().player_state == PlayerController.PlayerStates.JUMPING))
        {
            GameObject.Find("Player").GetComponent<PlayerController>().player_state = PlayerController.PlayerStates.IDLE;
            GameObject.Find("Player").GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            GameObject.Find("Player").GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ground" && GameObject.Find("Player").GetComponent<PlayerController>().player_state == PlayerController.PlayerStates.AIR)
        {
            GameObject.Find("Player").GetComponent<PlayerController>().player_state = PlayerController.PlayerStates.IDLE;
            GameObject.Find("Player").GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            GameObject.Find("Player").GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground" && GameObject.Find("Player").GetComponent<PlayerController>().player_state == PlayerController.PlayerStates.RUN)
        {
            GameObject.Find("Player").GetComponent<PlayerController>().player_state = PlayerController.PlayerStates.AIR;
        }
    }
}
