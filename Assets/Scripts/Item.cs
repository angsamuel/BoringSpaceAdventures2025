using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [Header("Items")]
    public int currentUses;
    public int maxUses;
    public float maxCooldown = 1f;
    float cooldownTimer = 0f;

    public void BaseUseItem(){
        if(currentUses <= 0){
            return;
        }

        if(cooldownTimer > 0){
            return;
        }

        cooldownTimer = maxCooldown;
        currentUses--;

        if(currentUses <= 0){
            currentUses = 0;
        }

        SubUseItem();
    }

    void Update(){
        cooldownTimer -= Time.deltaTime;
    }



    public abstract void SubUseItem();
    public virtual void Reload(){

    }
}
