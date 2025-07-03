using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIResourceManager : MonoBehaviour
{

    public List<OxygenRefillStation> oxygenRefillStations;

    public List<Creature> creatures;


    public static AIResourceManager singleton;
    void Awake(){
        if(singleton != null){
            Destroy(this.gameObject);
        }
        singleton = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public OxygenRefillStation GetNearestOxygenRefillStation(Vector3 position){
        if(oxygenRefillStations.Count < 1){
            return null;
        }

        OxygenRefillStation closest = oxygenRefillStations[0];
        for(int i = 0; i<oxygenRefillStations.Count; i++){
            if(Vector3.Distance(position,oxygenRefillStations[i].transform.position)
            < Vector3.Distance(position,closest.transform.position)){
                closest = oxygenRefillStations[i];
            }
        }
        return closest;
    }

    public List<Creature> GetCreaturesInRange(Creature creature, Vector3 position, float range){
        
        List<Creature> creaturesInRange = new List<Creature>();
        for(int i = 0; i<creatures.Count; i++){
            if(Vector3.Distance(position,creatures[i].transform.position) < range){
                if(creatures[i] == creature){
                    continue;
                }
            }
            creaturesInRange.Add(creatures[i]);
        }
        return creaturesInRange;
    }

    public void RegisterOxygenRefillStation(OxygenRefillStation station){
        oxygenRefillStations.Add(station);
    }
    public void RegisterCreature(Creature creature){
        creatures.Add(creature);
    }
}
