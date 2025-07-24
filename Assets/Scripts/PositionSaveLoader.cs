using UnityEngine;

public class PositionSaveLoader : MonoBehaviour
{
    public Creature playerCreature;



    public void SavePosition(){

        // SaveLoadBus.SaveFloat("playerX", playerCreature.transform.position.x);
        // SaveLoadBus.SaveFloat("playerY", playerCreature.transform.position.y);
        // SaveLoadBus.SaveFloat("playerZ", playerCreature.transform.position.z);

        SaveLoadBus.SaveVector3("playerPosition", playerCreature.transform.position);
        SaveLoadBus.SaveToFile();
    }

    public void LoadPosition(){
        playerCreature.GetComponent<CharacterController>().enabled = false;
        // playerCreature.transform.position = new Vector3(
        //     SaveLoadBus.LoadFloat("playerX", 0),
        //     SaveLoadBus.LoadFloat("playerY", 0),
        //     SaveLoadBus.LoadFloat("playerZ", 0)
        // );
        playerCreature.transform.position = SaveLoadBus.LoadVector3("playerPosition");
        playerCreature.GetComponent<CharacterController>().enabled = true;
    }
}
