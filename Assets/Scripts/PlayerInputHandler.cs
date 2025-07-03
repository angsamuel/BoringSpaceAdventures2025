using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerInputHandler : MonoBehaviour
{


    public Creature playerCreature;

    public List<Creature> creatures;

    public Transform cameraTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
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

        if(Input.GetKeyDown(KeyCode.Mouse0)){
            playerCreature.UseItem();
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
    }

    void LateUpdate(){
        playerCreature.AimItem(cameraTransform.rotation);
    }


}
