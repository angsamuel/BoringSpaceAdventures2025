using UnityEngine;
using System.Collections.Generic;

public class HealthPips : MonoBehaviour
{
    public GameObject pipPrefab;
    public Creature playerCreature;

    public List<Transform> pips;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        foreach(Transform t in pips){
            Destroy(t.gameObject);
        }
        pips = new List<Transform>();
        for(int i = 0; i<playerCreature.GetMaxHealth(); i++){
            pips.Add(Instantiate(pipPrefab,transform).transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i<pips.Count; i++){
            if(playerCreature.GetHealth() > i){
                pips[i].transform.GetChild(0).gameObject.SetActive(true);
            }else{
                pips[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}
