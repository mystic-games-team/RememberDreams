using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectGround : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            GameObject.Find("Player").GetComponent<PlayerController>().player_state = PlayerController.PlayerStates.IDLE;
        }
    }
}
