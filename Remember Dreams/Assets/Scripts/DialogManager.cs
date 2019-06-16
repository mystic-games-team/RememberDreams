using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public enum DialogType
    {
        NPC_TEST,

        NONE
    }

    public struct DialogData
    {
        public GameObject dialog_panel;
        public Sprite portrait_npc;
        public List<Interactivity.DialogNodes> dialog_node;
        public Interactivity.DialogNodes actual_node;
        public GameObject interactive_target;
    }
    [HideInInspector]
    public DialogData dialog_data;
    [HideInInspector]
    public bool active = false;
    private bool can_pass_dialog = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (can_pass_dialog)
            {
                if (Input.GetKeyDown(KeyCode.Space) && !dialog_data.dialog_panel.transform.Find("Option1").gameObject.activeInHierarchy)
                {
                    PerformNextPhrase();
                }
            }
            else
                can_pass_dialog = true;
            
        }
    }

    public void StartDialog(DialogData data)
    {
        // set struct
        dialog_data.dialog_panel = data.dialog_panel;
        dialog_data.dialog_node = data.dialog_node;
        dialog_data.portrait_npc = data.portrait_npc;
        dialog_data.actual_node = data.actual_node;
        dialog_data.interactive_target = data.interactive_target;

        // always ui elemtns must have as a parent canvas
        dialog_data.dialog_panel.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
        dialog_data.dialog_panel.transform.position = new Vector3(512, 99, dialog_data.dialog_panel.transform.position.z);

        // set the npc portrait image
        dialog_data.dialog_panel.transform.Find("PortraitNPC").GetComponent<Image>().sprite = dialog_data.portrait_npc;

        // set the first phrase
        dialog_data.dialog_panel.transform.Find("Text").GetComponent<Text>().text = dialog_data.actual_node.node_text[0];

        // disable options
        dialog_data.dialog_panel.transform.Find("Option1").gameObject.SetActive(false);
        dialog_data.dialog_panel.transform.Find("Option2").gameObject.SetActive(false);

        active = true;
    }
    public void PerformNextPhrase()
    {
         for (int i = 0; i <= dialog_data.actual_node.node_text.Count; ++i)
            {
                if (dialog_data.actual_node.node_text[i] == dialog_data.dialog_panel.transform.Find("Text").GetComponent<Text>().text)
                {
                    if (i + 1 < dialog_data.actual_node.node_text.Count) // there's another phrase before player can talk
                    {
                        dialog_data.dialog_panel.transform.Find("Text").GetComponent<Text>().text = dialog_data.actual_node.node_text[i + 1];
                        break;
                    }
                    else // there's no more npc phrase in this node, player options appear
                    {
                        GameObject obj1 = dialog_data.dialog_panel.transform.Find("Option1").gameObject; 
                        obj1.SetActive(true);
                        obj1.GetComponent<Text>().text = dialog_data.actual_node.options[0].option_text;
                        obj1.GetComponent<Button>().onClick.AddListener(delegate { PerformFirstNodePhrase(dialog_data.actual_node.options[0].next_node); });

                        
                        if (dialog_data.actual_node.options.Count > 1) // player has more than 1 option to talk
                        {
                            GameObject obj2 = dialog_data.dialog_panel.transform.Find("Option2").gameObject;
                            obj2.SetActive(true);
                            obj2.GetComponent<Text>().text = dialog_data.actual_node.options[1].option_text;
                            obj2.GetComponent<Button>().onClick.AddListener(delegate { PerformFirstNodePhrase(dialog_data.actual_node.options[1].next_node); }); // per passar parametres
                        }
                        break;
                    }
                }
            }
    }
    public void PerformFirstNodePhrase(int node)
    {
        if (node == 0) // conversation finished
        {
            if (dialog_data.dialog_panel.transform.Find("Option1").gameObject.activeInHierarchy)
            {
                active = false;
                dialog_data.interactive_target.GetComponent<Interactivity>().Invoke("SetToWaitingInteraction", 0.2F);
                Destroy(dialog_data.dialog_panel);
            }
        }
        else // another node is called
        {
            can_pass_dialog = false;
            for (int i = 0; i < dialog_data.dialog_node.Count; ++i) // set the actual_node, must be the one that has the same id that the function has
            {
                if (dialog_data.dialog_node[i].node_id == node)
                {
                    dialog_data.actual_node = dialog_data.dialog_node[i];
                    if (dialog_data.actual_node.safe_node)
                        dialog_data.interactive_target.GetComponent<Interactivity>().actual_node = dialog_data.actual_node;
                    break;
                }
            }

            // disable option buttons
            dialog_data.dialog_panel.transform.Find("Option1").gameObject.SetActive(false);
            dialog_data.dialog_panel.transform.Find("Option2").gameObject.SetActive(false);

            // set the first phrase of the node
            dialog_data.dialog_panel.transform.Find("Text").GetComponent<Text>().text = dialog_data.actual_node.node_text[0];
        }
        
    }
}
