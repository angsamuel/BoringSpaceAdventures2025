using UnityEngine;

public class OxygenMeter : MonoBehaviour
{

    public Creature playerCreature;
    public Transform barTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        barTransform.localScale = new Vector3(playerCreature.GetOxygenRatio(),1,1);
    }
}
