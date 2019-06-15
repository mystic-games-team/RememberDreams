using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


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

    public Texture portrait_npc;
    private GameObject copy_panel;
    [SerializeField]
    public List<DialogNodes> dialog_node = new List<DialogNodes>();

   

    // Start is called before the first frame update
    void Start()
    {
        copy_panel = dialog_panel;
    }

    private void Update()
    {
        if (interacting_state == InteractingStates.WAITING_INTERACTION)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                interacting_state = InteractingStates.INTERACTING;
                dialog_panel = Instantiate(copy_panel);
                dialog_panel.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
                dialog_panel.transform.position = new Vector3(512, 99, dialog_panel.transform.position.z);
                dialog_panel.transform.Find("Text").GetComponent<Text>().text = dialog_node[0].node_text[0];
            }
        }
        if (interacting_state == InteractingStates.INTERACTING)
        {

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
            if (dialog_panel != null)
                Destroy(dialog_panel);
        }
    }
}
