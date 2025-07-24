using UnityEngine;

public class ClimbingStake : Item
{

    [Header("Climbing Stake Objects")]
    public GameObject zoneVisual;
    public override void SubUseItem(Creature c)
    {
        GameObject newStake = Instantiate(this.gameObject,transform.position,Quaternion.identity);

        newStake.GetComponent<ClimbingStake>().CreateZone();
    }

    public void CreateZone(){
      GetComponent<Collider>().enabled = true;
      zoneVisual.SetActive(true);
    }

    public void OnTriggerEnter(Collider other){
    Debug.Log("Climbing Stake Collision!");
     if(other.GetComponent<Creature>() != null){
        other.GetComponent<Creature>().AddClimbStatus();
     }
   }

    public void OnTriggerExit(Collider other)
   {
        Debug.Log("Climbing Stake Exit!");
        if (other.GetComponent<Creature>() != null){
        other.GetComponent<Creature>().RemoveClimbStatus();
     }
   }
}
