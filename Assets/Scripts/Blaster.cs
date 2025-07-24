using UnityEngine;
using System.Collections;

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

    [Header("Aim Correction")]
    public Transform sightTransform; //use the camera for the player
    public LayerMask sightMask; //things we can hit

    //Audio
    [Header("Audio")]
    public AudioSource shootAudioSource;

    [Header("Spread")]
    public float currentSpread = 0;
    public float maxSpread = .95f;
    public float minSpread = .05f;
    public float spreadDecay = 2f;
    public float spreadPerShot = .1f;

    [Header("Kickback")]
    public Transform kickbackTransform;
    Vector3 defaultPos;
    public Vector3 kickbackPos;

    [Header("DownSights")]
    public Vector3 downSightsPosition;
    public Transform downSightsPivot;

    [Header("MuzzleFlash")]
    public GameObject flash;
    public float flashTime = .02f;
    public Vector2 flashRotation;
    public Vector2 flashScale;

    [Header("Shells")]
    public Transform shellSpawnPoint;
    public GameObject shellPrefab;
    public float shellSpeed = 10f;

    void Start(){
        defaultPos = kickbackTransform.localPosition;
        //GenerateBlaster();
    }



    public void DownSightsUpdate(){
        float easedProgress = Mathf.SmoothStep(0, 1, downSightsProgress);
        downSightsPivot.localPosition = Vector3.Lerp(Vector3.zero, downSightsPosition, easedProgress);
    }


    bool kickbackActive = false;
    public void Kickback(){
        kickbackTransform.localPosition = Vector3.Lerp(defaultPos, kickbackPos, cooldownTimer / maxCooldown);
    }

    bool flashing = false;
    public void MuzzleFlash(){
        if(flashing){
            return;
        }
        flashing = true;
        flash.SetActive(true);
        flash.transform.localEulerAngles = new Vector3(
            Random.Range(flashRotation.x, flashRotation.y),
            flash.transform.localEulerAngles.y,
            flash.transform.localEulerAngles.z
        );
        flash.transform.localScale = Vector3.one * Random.Range(flashScale.x,flashScale.y);
        StartCoroutine(MuzzleFlashRoutine());

        IEnumerator MuzzleFlashRoutine(){
            yield return new WaitForSeconds(flashTime);
            flash.SetActive(false);
            flashing = false;
        }
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


    public override void SubUseItem(Creature c){
        //Shoot!
        Debug.Log("Shoot!");
        shootAudioSource.pitch = Random.Range(.9f,1.1f);
        shootAudioSource.PlayOneShot(shootAudioSource.clip);


        GameObject newProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        newProjectile.transform.localScale = Vector3.one * projectileScale;






        newProjectile.GetComponent<Projectile>().SetDamage(projectileDamage);

        //use sight transform
        Vector3 shootDirection = projectileSpawnPoint.forward;

        if(sightTransform != null){
            Ray ray = new Ray(sightTransform.position, sightTransform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, sightMask))
            {
                Debug.Log("RAY HIT");
                shootDirection = (hit.point - projectileSpawnPoint.position).normalized;

                //check if hit is too close
                if(hit.distance < 3f){
                    Debug.Log("HIT TOO CLOSE");
                    shootDirection = projectileSpawnPoint.forward;
                }
            }
            else
            {
                shootDirection = sightTransform.forward;
            }
        }


        //apply spread
        Vector3 spreadDirection = shootDirection;
        spreadDirection += Random.insideUnitSphere * currentSpread * (1f - downSightsProgress); //accurate when down sights
        spreadDirection.Normalize();


        //newProjectile.GetComponent<Rigidbody>().linearVelocity = projectileSpawnPoint.forward * projectileSpeed;
        newProjectile.GetComponent<Rigidbody>().linearVelocity = spreadDirection * projectileSpeed;
        Destroy(newProjectile, 20f);




        if (c.isPlayer){
            PlayerInputHandler.singleton.ApplyRecoil(GenerateRecoil(), recoilRecoverySpeed);
        }else{
            //handle this case for NPCs later
        }


        spreadDecayBlockers = 1;
        ApplySpread();
        EjectShell(c);
        MuzzleFlash();

    }

    public void EjectShell(Creature c){
       GameObject newShell = Instantiate(shellPrefab,shellSpawnPoint.position, shellSpawnPoint.transform.rotation);
       newShell.GetComponent<Rigidbody>().linearVelocity = shellSpawnPoint.transform.right * shellSpeed + c.GetCachedMove();
       Destroy(newShell,1);
    }

    public void ApplySpread(){

        currentSpread += spreadPerShot;
        currentSpread = Mathf.Clamp(currentSpread, minSpread, maxSpread);
    }

    int spreadDecayBlockers = 0;
    public void DecaySpread(){
        if(cooldownTimer > 0){
            return;
        }
        if(spreadDecayBlockers > 0){
            spreadDecayBlockers--;
            return;
        }
        currentSpread = Mathf.MoveTowards(currentSpread, minSpread, spreadDecay * Time.deltaTime);
    }
    public float GetCurrentSpread(){
        return currentSpread;
    }

    protected override void Update(){
        base.Update();
        Kickback();
        DecaySpread();
        Aiming();
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
