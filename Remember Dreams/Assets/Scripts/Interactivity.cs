using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactivity : MonoBehaviour
{
    enum InteractingStates
    {
        NO_RANGE_TO_INTERACT,
        WAITING_INTERACTION,
        INTERACTING,

        NONE
    }

    public DialogManager.DialogType dialog_type = DialogManager.DialogType.NONE;
    private InteractingStates interacting_state = InteractingStates.NO_RANGE_TO_INTERACT;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (interacting_state == InteractingStates.WAITING_INTERACTION)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                interacting_state = InteractingStates.INTERACTING;
                Debug.Log("Interacting");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interacting_state = InteractingStates.WAITING_INTERACTION;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interacting_state = InteractingStates.NO_RANGE_TO_INTERACT;
        }
    }
}
