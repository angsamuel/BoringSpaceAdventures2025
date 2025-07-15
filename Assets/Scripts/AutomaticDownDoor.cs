using UnityEngine;
using System.Collections;
public class AutomaticDownDoor : MonoBehaviour
{

    public enum DoorState{opening, closing, open, closed}
    DoorState state = DoorState.closed;

    public Transform door;
    public Vector3 upPosition;
    public Vector3 downPosition;
    public float transitionTime = 1f;

    int creaturesByDoor = 0;


    float doorProgress = 1;
    float doorDirection = 0;

    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Creature")){
            creaturesByDoor++;
            OpenDoor();
        }
    }

    void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("Creature")){
            Debug.Log("Exit");
            creaturesByDoor--;
            CloseDoor();
        }
    }

    void OpenDoor(){
        doorDirection = -1;
    }
    void CloseDoor(){
        if(creaturesByDoor > 0){
            return;
        }
        doorDirection = 1;
    }

    void Update(){
        if(doorDirection == 0){
            return;
        }

        doorProgress += Time.deltaTime * doorDirection / transitionTime;
        door.localPosition = Vector3.Lerp(downPosition,upPosition,doorProgress);

        if(doorProgress >= 1){
            doorProgress = 1;
            doorDirection = 0;
            door.localPosition = upPosition;
        }
        if(doorProgress <= 0){
            doorProgress = 0;
            doorDirection = 0;
            door.localPosition = downPosition;
        }

    }






}
