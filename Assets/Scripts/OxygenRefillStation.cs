using UnityEngine;

public class OxygenRefillStation : MonoBehaviour
{
    public void OnTriggerEnter(Collider other){
        if(other.GetComponent<Creature>() != null){
            other.GetComponent<Creature>().RefillOxygen();
        }
    }
}
