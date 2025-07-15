using UnityEngine;
using System.Collections.Generic;
public class Creature : MonoBehaviour
{
    public float damageMultiplier = .5f;


    public float speed = 10f;
    public float jumpForce = 10f;

    public int health = 3;
    public int maxHealth = 3;

    public float oxygenRefillChance = .5f;
    public float maxOxygen = 100f;
    public float oxygen = 100f;


    CharacterController cc;

    public float gravity = -9.81f;
    public Vector3 currentGravity = Vector3.zero;

    public Transform bodyTransform;
    public float rotationSpeed = 10f;


    int canClimbStaus = 0;
    float climbAngle = 64f;
    float defaultAngle = 45f;

    [Header("Items")]
    public Transform itemPivot;
    public Item equippedItem;
    public List<Item> inventory;
    int currentIndex = 0;

    [Header("Special")]
    public bool isPlayer = false;





    void Awake(){

        cc = GetComponent<CharacterController>();
        SelectInventoryItem(0);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        oxygen = maxOxygen;
        AIResourceManager.singleton.RegisterCreature(this);
        //transform.position = transform.position + new Vector3(10, 0, 0);
    }

    public void SimulateGravity()
    {

        currentGravity.y += gravity * Time.deltaTime;

        cc.Move(currentGravity * Time.deltaTime);

        if(cc.isGrounded){
            currentGravity = new Vector3(0,-1,0);
        }
    }

    public void Jump()
    {
        currentGravity.y = jumpForce;
        ConsumeOxygen(10);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = transform.position + (new Vector3(10,0,0) * speed * Time.deltaTime );
        SimulateGravity();

    }

    // void FaceTransform(){
    //     bodyTransform.rotation = Quaternion.LookRotation(facingTransform.position - transform.position);
    // }



    public void Move(Vector3 direction){

        direction.y = 0;



        if (direction == Vector3.zero) //fixed
        { //this was the problem
            return;
        }

        direction = direction.normalized;
        //consume a little bit of oxygen
        //transform.position = transform.position + direction * speed * Time.deltaTime;
        cc.Move(direction * speed * Time.deltaTime);

        //oxygen drain
        ConsumeOxygen(speed * Time.deltaTime);

        // Quaternion lookRotation = Quaternion.LookRotation(direction);
        // bodyTransform.rotation = Quaternion.Lerp(bodyTransform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    public void MoveToward(Vector3 target){
        Vector3 direction = target - transform.position;
        Move(direction);
    }



    public void ConsumeOxygen(float amount){
        oxygen -= amount;
        if(oxygen <= 0){
            oxygen = 0;
            RefillOxygen(25f);
            if(Random.Range(0f,1f) < oxygenRefillChance){
                TakeDamage(1);
            }
        }
    }
    public void RefillOxygen(float amount = 100f){
        oxygen += amount;
        if(oxygen > maxOxygen){
            oxygen = maxOxygen;
        }
    }






    public void SetHealth(int newHealth)
    {
        if (newHealth < 0)
        {
            newHealth = 0;
        }
        health = newHealth;
    }

    public int GetHealth()
    {
        return health;
    }

    public float GetOxygen(){
        return oxygen;
    }

    public void TakeDamage(int damage){
        health -= damage;
        if(health <= 0){
            health = 0;
        }
    }

    public bool IsAlive(){
        return health > 0;
    }

    //climbing code
    public void AddClimbStatus(){
        canClimbStaus++;
        Debug.Log("Climb Status: " + canClimbStaus);
        GetComponent<CharacterController>().slopeLimit = climbAngle;
    }

    public void RemoveClimbStatus(){
        canClimbStaus--;
        Debug.Log("Climb Status: " + canClimbStaus);
        if(canClimbStaus <= 0){
            canClimbStaus = 0;
            GetComponent<CharacterController>().slopeLimit = defaultAngle;
        }
    }

    public void AimItem(Quaternion rotation){
        itemPivot.rotation = rotation;
    }
    public void AimItemToward(Vector3 target){
        AimItem(Quaternion.LookRotation(target - transform.position));
    }

    public void UseItem(){
        if(equippedItem != null){
            equippedItem.BaseUseItem();
        }
    }

    public void ReloadItem(){
        if(equippedItem != null){
            equippedItem.Reload();
        }
    }


    void SelectInventoryItem(int index){
        if(inventory.Count <= 0){
            return;
        }

        for(int i = 0 ; i<inventory.Count; i++){
            inventory[i].gameObject.SetActive(false);
        }
        if(index < 0){
            index = inventory.Count - 1;
        }
        if(index >= inventory.Count){
            index = 0;
        }

        inventory[index].gameObject.SetActive(true);
        currentIndex = index;
        equippedItem = inventory[index];
    }

    public void NextInventoryItem(){
        SelectInventoryItem(currentIndex + 1);
    }
    public void PreviousInventoryItem(){
        SelectInventoryItem(currentIndex - 1);
    }

    public float GetPerFrameDistance(){
        return speed * Time.deltaTime;
    }


}
