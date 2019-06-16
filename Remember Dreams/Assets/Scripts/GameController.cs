using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameController : MonoBehaviour
{
    [System.Serializable]
    public class NPC_DataSave
    {
        public NPC_DataSave (DialogManager.DialogType type, int node)
        {
            dialog_type = type;
            actual_node = node;
        }
        public DialogManager.DialogType dialog_type;
        public int actual_node;
    }

    private List<NPC_DataSave> npc_data = new List<NPC_DataSave>();

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        Debug.Log(Application.persistentDataPath);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10))
            SaveGame();
        if (Input.GetKeyDown(KeyCode.F11))
            LoadGame();
    }

    public void SaveNPC(int node_to_remember, DialogManager.DialogType type) // new start node for that npc
    {
        for (int i = 0; i < npc_data.Count;++i)
        {
            if (npc_data[i].dialog_type == type)
            {
                npc_data[i].actual_node = node_to_remember;
            }
        }
    }
    public int SetNPCStartNode(DialogManager.DialogType type, int first_node) // loking for which node has to start
    {
        for (int i = 0; i < npc_data.Count; ++i)
        {
            if (npc_data[i].dialog_type == type)
            {
                return npc_data[i].actual_node;
            }
        }
        npc_data.Add(new NPC_DataSave(type, first_node));
        return -1;
    }

    private void RefreshNodeNPC ()
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("Interactive"); // get all npcs
        for (int i = 0; i < npcs.Length; ++i) 
        {
            if (npc_data[i].dialog_type == npcs[i].GetComponent<Interactivity>().dialog_type)
            {
                if (npc_data[i].actual_node != npcs[i].GetComponent<Interactivity>().actual_node.node_id) //refresh the node
                {
                    //npcs[i].GetComponent<Interactivity>().actual_node.node_id = npc_data[i].actual_node;
                    for (int j = 0; j < npcs[i].GetComponent<Interactivity>().dialog_node.Count; ++j) 
                    {
                        if (npcs[i].GetComponent<Interactivity>().dialog_node[j].node_id == npc_data[i].actual_node)
                        {
                            npcs[i].GetComponent<Interactivity>().actual_node = npcs[i].GetComponent<Interactivity>().dialog_node[j];
                            break;
                        }
                    }
                }
            }
        }
    }

    private Save CreateSaveGameObject() //save values in another class
    {
        Save save = new Save();

        save.npc_save_list = npc_data;

        return save;
    }

    public void SaveGame()
    {
        Save save = CreateSaveGameObject();
       
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save_game.txt");
        bf.Serialize(file, save);
        file.Close();

    }


    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/save_game.txt"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save_game.txt", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            // start loading
            
            // NPC
            npc_data = save.npc_save_list; // we just load NPC xdd
            RefreshNodeNPC(); 
            //

        }
    }
}
