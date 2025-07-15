using UnityEngine;

public class Blaster : Item
{

    [Header("Blaster Stats")]
    public float projectileSpeed = 20f;
    public int projectileDamage = 1;

    public float projectileScale = 1f;
    public int maxMagazines = 10;
    int magazinesLeft = 10;


    [Header("Blaster Objects")]
    public Transform projectileSpawnPoint;
    public GameObject projectilePrefab;

    public Transform aimTransform; //use this to choose final projectile position.


    [Header("Generation Settings")]
    public int seed;
    public Vector2 speedRange;
    public Vector2 damageRange;
    public Vector2 ammoInMagazineRange;
    public Vector2 spareMagazineRange;
    public Vector2 fireRateRange;
    public Vector2 projectileScaleRange;

    //speed
    //damage
    //ammunition
    //magazine size
    //fire rate

    //accuracy


    void Start(){
        GenerateBlaster();
    }

    public void GenerateBlaster()
    {
        Random.InitState(seed);
        projectileSpeed = Random.Range(speedRange.x, speedRange.y);
        projectileDamage = (int)Random.Range(damageRange.x, damageRange.y);

        maxCooldown = 1f / Random.Range(fireRateRange.x, fireRateRange.y);
        maxUses = (int)Random.Range(ammoInMagazineRange.x, ammoInMagazineRange.y);
        currentUses = maxUses;

        maxMagazines = (int)Random.Range(spareMagazineRange.x, spareMagazineRange.y);
        magazinesLeft = maxMagazines;

        projectileScale = Random.Range(projectileScaleRange.x, projectileScaleRange.y);

    }


    public override void SubUseItem(){
        //Shoot!
        Debug.Log("Shoot!");
        GameObject newProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        newProjectile.transform.localScale = Vector3.one * projectileScale;
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
