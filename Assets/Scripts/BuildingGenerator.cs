using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BuildingGenerator : MonoBehaviour
{
    public HashSet<Vector3Int> interiorLocations;
    public HashSet<Vector3Int> wallLocations;
    public HashSet<Vector3Int> doorLocations;

    [Header("Config")]

    public int buildingWidth = 25;
    public int buildingHeight = 25;
    public int buildingBuffer = 2;
    public int rooms = 3;
    public int roomAttempts = 100;
    public Vector2Int roomSizeRange = new Vector2Int(3,6);
    public int maxDoors = 6;

    [Header("Prefabs")]
    public int prefabSize = 2;
    public GameObject wallCrossPrefab;
    public GameObject doorPrefab;
    public GameObject floorPrefab;
    public GameObject roofPrefab;
    void Awake(){
        interiorLocations = new HashSet<Vector3Int>();
        wallLocations = new HashSet<Vector3Int>();
        doorLocations = new HashSet<Vector3Int>();
    }
    void Start(){
        //decide on interior locations
        AddInteriors();
        AddWalls();
        AddDoors();
        //
        //
        //
        //
        PlaceFloorsAndRoofs();
        PlaceWalls();
        PlaceDoors();
    }

    public void AddInteriors(){

        for(int i = 0; i<rooms; i++){

            bool roomPlaced = false;
            int currentAttempts = 0;

            while(!roomPlaced && currentAttempts < roomAttempts ){
                int roomWidth = Random.Range(roomSizeRange.x, roomSizeRange.y);
                int roomHeight = Random.Range(roomSizeRange.x, roomSizeRange.y);

                int x = Random.Range(buildingBuffer, buildingWidth - roomWidth - buildingBuffer);
                int z = Random.Range(buildingBuffer, buildingHeight - roomHeight - buildingBuffer);

                if (CanConnectToExistingRooms(x, z, roomWidth, roomHeight))
                {
                    DrawRoom(x, z, roomWidth, roomHeight);
                    roomPlaced = true;
                }
                currentAttempts++;
            }
        }
    }

    public bool CanConnectToExistingRooms(int x, int z, int roomWidth, int roomHeight){
        if (interiorLocations.Count == 0)
        {
            return true;
        }
        for (int i = 0; i<roomWidth; i++){
            for(int j = 0; j<roomHeight; j++){
                Vector3Int location = new Vector3Int(x+i,0,z+j);
                if(interiorLocations.Contains(location)){
                    return true;
                }
            }
        }
        return false;
    }

    public void DrawRoom(int x, int z, int roomWidth, int roomHeight){
        for(int i = 0; i<roomWidth; i++){
            for(int j = 0; j<roomHeight; j++){
                Vector3Int location = new Vector3Int(x+i,0,z+j);
                interiorLocations.Add(location);
            }
        }
    }

    public void PlaceFloorsAndRoofs(){
        foreach(Vector3Int location in interiorLocations){
            Instantiate(floorPrefab, location * prefabSize, Quaternion.identity);
            Instantiate(roofPrefab, location * prefabSize + new Vector3(0,prefabSize,0), Quaternion.identity);
        }
    }

    public void PlaceDoors(){
        foreach(Vector3Int location in doorLocations){
            GameObject newDoor =Instantiate(doorPrefab, location * prefabSize, Quaternion.identity);
            if(wallLocations.Contains(location + new Vector3Int(1,0,0))){
                newDoor.transform.Rotate(0, 90, 0);
            }
        }
    }

    public void PlaceWalls(){
        foreach(Vector3Int location in wallLocations){
            if(doorLocations.Contains(location)){
                continue;
            }


            BuildingCross bc = Instantiate(wallCrossPrefab, location*prefabSize, Quaternion.identity).GetComponent<BuildingCross>();
            bool up = false;
            bool down = false;
            bool left = false;
            bool right = false;
            if(wallLocations.Contains(location + new Vector3Int(1,0,0))){
                right = true;
            }
            if (wallLocations.Contains(location + new Vector3Int(-1, 0, 0))){
                left = true;
            }
            if (wallLocations.Contains(location + new Vector3Int(0, 0, 1))){
                up = true;
            }
            if (wallLocations.Contains(location + new Vector3Int(0, 0, -1))){
                down = true;
            }

            bc.ChooseWalls(up,down,left,right);

        }
    }

    public void AddWalls(){
        for(int x = 0; x<buildingWidth; x++){
            for(int z = 0; z<buildingHeight; z++){

                if(interiorLocations.Contains(new Vector3Int(x,0,z))){ //don't replace interior with wall
                    continue;
                }

                for(int rx = -1; rx<2; rx++){ //check each direction
                    for(int rz = -1; rz<2; rz++){
                        if(rx == 0 && rz == 0){ //don't check for interior where we are
                            continue;
                        }
                        if(interiorLocations.Contains(new Vector3Int(x+rx,0,z+rz))){
                            wallLocations.Add(new Vector3Int(x,0,z));
                        }
                    }
                }



            }
        }
    }

    bool OneInteriorLocation(Vector3Int direction1, Vector3Int direction2){
        return interiorLocations.Contains(direction1) && !interiorLocations.Contains(direction2) && !wallLocations.Contains(direction2) ||
            interiorLocations.Contains(direction2) && !interiorLocations.Contains(direction1) && !wallLocations.Contains(direction1);
    }

    public void AddDoors(){
        List<Vector3Int> possibleDoorLocations = new List<Vector3Int>();

        foreach(Vector3Int wallPos in wallLocations){
            Vector3Int north = wallPos + new Vector3Int(0,0,1);
            Vector3Int south = wallPos + new Vector3Int(0,0,-1);
            Vector3Int east = wallPos + new Vector3Int(1,0,0);
            Vector3Int west = wallPos + new Vector3Int(-1,0,0);


            bool nsIsValid = OneInteriorLocation(north, south);
            bool ewIsValid = OneInteriorLocation(east, west);

            if(nsIsValid || ewIsValid){
                possibleDoorLocations.Add(wallPos);
            }
    }
        for(int i = 0; i<maxDoors; i++){
            int randIndex = Random.Range(0,possibleDoorLocations.Count);
            Vector3Int doorLocation = possibleDoorLocations[randIndex];
            possibleDoorLocations.RemoveAt(randIndex);
            Debug.Log("Door Location Added: " + doorLocation);
            doorLocations.Add(doorLocation);
        }

    }


}
