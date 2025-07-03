using UnityEngine;

public class Blaster : Item
{

    [Header("Blaster Stats")]
    public float projectileSpeed = 20f;
    public int projectileDamage = 1;


    public int maxMagazines = 10;
    int magazinesLeft = 10;


    [Header("Blaster Objects")]
    public Transform projectileSpawnPoint;
    public GameObject projectilePrefab;

    public Transform aimTransform; //use this to choose final projectile position.

    void Start(){
        magazinesLeft = maxMagazines;
    }


    public override void SubUseItem(){
        //Shoot!
        Debug.Log("Shoot!");
        GameObject newProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        newProjectile.GetComponent<Projectile>().SetDamage(projectileDamage);
        newProjectile.GetComponent<Rigidbody>().linearVelocity = projectileSpawnPoint.forward * projectileSpeed;
        Destroy(newProjectile, 20f);
    }

    public override void Reload(){
        if(magazinesLeft <= 0){
            return;
        }
        currentUses = maxUses;
        magazinesLeft -= 1;
    }

    public override bool NeedsReload(){
        if(currentUses < 1){
            return true;
        }
        return false;
    }
}
