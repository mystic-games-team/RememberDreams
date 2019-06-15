using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Interactivity : MonoBehaviour
{
    enum InteractingStates
    {
        NO_RANGE_TO_INTERACT,
        WAITING_INTERACTION,
        INTERACTING,

        NONE
    }

    [System.Serializable]
    public struct DialogOptions
    {
        public string option_text;
        public int next_node;
    }

    [System.Serializable]
    public struct DialogNodes
    {
        public int node_id;
        public List<string> node_text;
        public List<DialogOptions> options;

    }

    public GameObject display_text = null;

    public GameObject dialog_panel;
    public DialogManager.DialogType dialog_type = DialogManager.DialogType.NONE;
    private InteractingStates interacting_state = InteractingStates.NO_RANGE_TO_INTERACT;

    public Sprite portrait_npc;
    private GameObject copy_panel;
    [SerializeField]
    public List<DialogNodes> dialog_node = new List<DialogNodes>();

    private DialogNodes actual_node;

    // Start is called before the first frame update
    public void Start()
    {
        copy_panel = dialog_panel;
        actual_node = dialog_node[0];
    }

    public void Update()
    {
        if (interacting_state == InteractingStates.WAITING_INTERACTION)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                interacting_state = InteractingStates.INTERACTING;
                dialog_panel = Instantiate(copy_panel);

                DialogManager.DialogData data;
                data.actual_node = actual_node;
                data.dialog_node = dialog_node;
                data.dialog_panel = dialog_panel;
                data.interactive_target = gameObject;
                data.portrait_npc = portrait_npc;
                GameObject.Find("DialogManager").SendMessage("StartDialog", data);
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interacting_state = InteractingStates.WAITING_INTERACTION;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interacting_state = InteractingStates.NO_RANGE_TO_INTERACT;
            if (dialog_panel != null)
                Destroy(dialog_panel);
        }
    }
}
