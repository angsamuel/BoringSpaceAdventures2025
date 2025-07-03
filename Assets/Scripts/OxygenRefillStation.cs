using UnityEngine;

public class OxygenRefillStation : MonoBehaviour
{

    void Start(){
        AIResourceManager.singleton.RegisterOxygenRefillStation(this);
    }
    public void OnTriggerEnter(Collider other){
        if(other.GetComponent<Creature>() != null){
            other.GetComponent<Creature>().RefillOxygen();
        }
    }
}
