using UnityEngine;

public class TurretAI : MonoBehaviour
{
    public GameObject targetObject;
    public Creature myCreature;
    public void Start(){

    }
    void Update(){
        if(myCreature.GetHealth() <= 0){
            Destroy(this.gameObject);
        }
        myCreature.AimItemToward(targetObject.transform.position);
        myCreature.UseItem();
    }
}
