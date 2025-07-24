using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class PlayerInputHandler : MonoBehaviour
{


    public static PlayerInputHandler singleton;
    public Creature playerCreature;

    public List<Creature> creatures;

    public Transform cameraTransform;

    [Header("Look Settings")]

    public float mouseSensitivity = 100f;
    public float verticalClamp = 85f;

    float pitch = 0f; // Up/down rotation (camera)
    float yaw = 0f;   // Left/right rotation (player)
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    Vector2 currentRecoil = Vector2.zero;
    Vector2 recoilVelocity = Vector2.zero;
    float recoilRecoverySpeed = 10f;

    public CameraTilt cameraTilt;


    void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    void Awake(){
        singleton = this;
    }


    // Update is called once per frame
    void Update()
    {

        Vector3 movement = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.A))
        {
            movement += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement += new Vector3(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            movement += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement += new Vector3(0, 0, -1);
        }

        cameraTilt.Tilt(movement.x);

        Vector3 cameraMoveForward = cameraTransform.forward * movement.z;
        Vector3 cameraMoveRight = cameraTransform.right * movement.x;
        Vector3 cameraAdjustedMovement = cameraMoveForward + cameraMoveRight;

        cameraAdjustedMovement.y = 0;

        playerCreature.Move(cameraAdjustedMovement);

        for(int i = 0; i<creatures.Count; i++){
            creatures[i].Move(movement);
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            playerCreature.Jump();
        }

        if(Input.GetKey(KeyCode.Mouse0)){
            playerCreature.UseItem();
        }

        if(Input.GetKey(KeyCode.Mouse1)){
            playerCreature.AimDownSights();
        }else{
            playerCreature.CancelDownSights();
        }

        if(Input.GetKeyDown(KeyCode.R)){
            playerCreature.ReloadItem();
        }

        if(Input.GetKeyDown(KeyCode.Q)){
            playerCreature.PreviousInventoryItem();
        }
        if(Input.GetKeyDown(KeyCode.E)){
            playerCreature.NextInventoryItem();
        }

        //TODO Remove
        if(Input.GetKeyDown(KeyCode.O)){
            GetComponent<PositionSaveLoader>().SavePosition();
        }
        if(Input.GetKeyDown(KeyCode.P)){
            GetComponent<PositionSaveLoader>().LoadPosition();
        }


        //Camera Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        currentRecoil = Vector2.SmoothDamp(currentRecoil,Vector2.zero, ref recoilVelocity, 1f/recoilRecoverySpeed);

        yaw += mouseX + currentRecoil.x;
        pitch -= mouseY + currentRecoil.y;
        pitch = Mathf.Clamp(pitch, -verticalClamp, verticalClamp);

        playerCreature.AimItem(Quaternion.Euler(pitch, 0f, 0f));
        playerCreature.transform.rotation = Quaternion.Euler(0f,yaw,0f);
    }


    public void ApplyRecoil(Vector2 recoil, float recoverySpeed){
        currentRecoil += recoil;
        recoilRecoverySpeed = recoverySpeed;
    }

    void LateUpdate(){
        //playerCreature.AimItem(cameraTransform.rotation);
    }


}
