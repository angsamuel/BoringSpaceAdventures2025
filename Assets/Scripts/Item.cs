using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [Header("Items")]
    public int currentUses;
    public int maxUses;
    public float maxCooldown = 1f;
    protected float cooldownTimer = 0f;

    [Header("Recoil")]
    public Vector2 verticalRecoil;
    public Vector2 horizontalRecoil;
    public float recoilRecoverySpeed = 10f;
    public float downSightsReduction = .5f;

    [Header("Sights")]
    public bool downSights = false;
    public float downSightsProgress = 0f;
    public float downSightsTime = .25f;

    public void BaseUseItem(Creature c){
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

        SubUseItem(c);
    }

    protected virtual void Update(){

        cooldownTimer -= Time.deltaTime;
    }

    public void AimDownSights(){
        downSightsProgress += (1/downSightsTime) * Time.deltaTime;
        if(downSightsProgress > 1){
            downSightsProgress = 1;
        }
    }

    public void CancelDownSights()
    {
        downSightsProgress -= (1/downSightsTime) * Time.deltaTime;
        if(downSightsProgress < 0){
            downSightsProgress = 0;
        }
    }


    public abstract void SubUseItem(Creature c);

    public virtual bool NeedsReload(){
        return false;
    }
    public virtual void Reload(){

    }


    public Vector2 GenerateRecoil(){

        return new Vector2(
            Random.Range(horizontalRecoil.x, horizontalRecoil.y) * (1f - (downSightsReduction * downSightsProgress)),
            Random.Range(verticalRecoil.x, verticalRecoil.y) * (1f - (downSightsReduction * downSightsProgress))
            );
    }


}
