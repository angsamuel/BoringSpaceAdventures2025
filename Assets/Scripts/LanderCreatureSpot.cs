using UnityEngine;

public class LanderCreatureSpot : MonoBehaviour
{
    public Lander lander;
    public Beacon beacon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other){
        if(!beacon.IsOn()){
            return;
        }
        if(other.CompareTag("Player")){
            lander.TakeOff();
        }
    }
}
