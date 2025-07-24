using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{

    public HashSet<Vector3Int> interiorLocations;
    public HashSet<Vector3Int> wallLocations;
    public HashSet<Vector3Int> doorLocations;

    [Header("Config")]
    public int blockSize = 2;
    public int dubngeonWidth = 25;
    public int dungeonHeight = 25;
    public int rooms = 3;
    public int maxDoors = 6;

    [Header("Prefabs")]
    public GameObject wallCrossPrefab;
    public GameObject interiorMarkerPrefab;
    public GameObject doorPrefab; //rotate 90 degrees for vertical door


    void Awake(){
        interiorLocations = new HashSet<Vector3Int>();
        wallLocations = new HashSet<Vector3Int>();
        doorLocations = new HashSet<Vector3Int>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        AddInteriors();
        AddWallsToInterior();
        //Add a place doors step
        PlaceDoors();
        PlaceDungeonWalls();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddInteriors()
    {
        for (int i = 0; i < rooms; i++)
        {
            int attempts = 0;
            bool roomPlaced = false;

            while (!roomPlaced && attempts < 100) //cap attempts
            {
                int width = Random.Range(3, 7);
                int height = Random.Range(3, 7);
                int x = Random.Range(2, dubngeonWidth - width - 2);
                int z = Random.Range(2, dungeonHeight - height - 2);

                if (CanConnectToExistingRooms(x, z, width, height))
                {
                    DrawRoom(x, z, width, height);
                    roomPlaced = true;
                }
                attempts++;
            }
        }
    }

    public void DrawRoom(int x, int z, int width, int height)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                interiorLocations.Add(new Vector3Int(x + i, 0, z + j));
                Instantiate(interiorMarkerPrefab, new Vector3Int((x + i) * blockSize, 0, (z + j) * blockSize), Quaternion.identity);
            }
        }
    }



    public void AddWallsToInterior(){
        for(int x = 0; x<dubngeonWidth; x++){
            for(int z = 0; z<dungeonHeight; z++){
                if(!interiorLocations.Contains(new Vector3Int(x, 0, z))){
                    continue;
                }

                for(int rx = -1; rx<2; rx++){
                    for(int rz = -1; rz<2; rz++){
                        if(rx==0 && rz==0){
                            continue;
                        }

                        if(interiorLocations.Contains(new Vector3Int(x + rx, 0, z + rz))){
                            continue;
                        }

                        wallLocations.Add(new Vector3Int(x + rx, 0, z + rz));
                    }
            }
        }
    }
    }



    public void PlaceDungeonWalls(){
        for(int x = 0; x<dubngeonWidth; x++){
            for(int z = 0; z<dungeonHeight; z++){

                if(!wallLocations.Contains(new Vector3Int(x, 0, z))){ //only place wall here if we want one
                    continue;
                }
                if(doorLocations.Contains(new Vector3Int(x, 0, z))){//don't override an existing door
                    continue;
                }

                GameObject newWall = Instantiate(wallCrossPrefab, new Vector3(x* blockSize, 0, z* blockSize), Quaternion.identity);

                newWall.GetComponent<BuildingCross>().ChooseWalls( //if no location in each direction, make sure to throw up a wall
                    wallLocations.Contains(new Vector3Int(x,0, z + 1)),
                    wallLocations.Contains(new Vector3Int(x,0, z - 1)),
                    wallLocations.Contains(new Vector3Int(x - 1,0, z)),
                    wallLocations.Contains(new Vector3Int(x + 1,0, z))
                );

            }
        }
    }

    public void PlaceDoors()
    {
        List<(Vector3Int position, Quaternion rotation)> validDoors = new List<(Vector3Int, Quaternion)>();

        foreach (var wallPos in wallLocations)
        {
            Vector3Int north = wallPos + new Vector3Int(0, 0, 1);
            Vector3Int south = wallPos + new Vector3Int(0, 0, -1);
            Vector3Int east = wallPos + new Vector3Int(1, 0, 0);
            Vector3Int west = wallPos + new Vector3Int(-1, 0, 0);

            bool nsValid =
                (interiorLocations.Contains(north) && !interiorLocations.Contains(south) && !wallLocations.Contains(south)) ||
                (interiorLocations.Contains(south) && !interiorLocations.Contains(north) && !wallLocations.Contains(north));

            bool ewValid =
                (interiorLocations.Contains(east) && !interiorLocations.Contains(west) && !wallLocations.Contains(west)) ||
                (interiorLocations.Contains(west) && !interiorLocations.Contains(east) && !wallLocations.Contains(east));

            if (nsValid || ewValid)
            {
                Quaternion rotation;
                if (ewValid)
                {
                    rotation = Quaternion.identity;
                }
                else
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                validDoors.Add((wallPos, rotation));
            }
        }

        int doorsToPlace = Mathf.Min(maxDoors, validDoors.Count);
        for (int i = 0; i < doorsToPlace; i++)
        {
            int randIndex = Random.Range(i, validDoors.Count);
            var (pos, rot) = validDoors[randIndex];
            validDoors.RemoveAt(randIndex);
            Instantiate(doorPrefab, pos * blockSize, rot);
            doorLocations.Add(pos);
        }
    }


    private bool CanConnectToExistingRooms(int x, int z, int width, int height)
    {
        if (interiorLocations.Count == 0) return true; // First room is always valid

        //see if room is adjacent to anything else
        for (int i = -1; i <= width; i++)
        {
            for (int j = -1; j <= height; j++)
            {
                Vector3Int checkPos = new Vector3Int(x + i, 0, z + j);
                if (interiorLocations.Contains(checkPos))
                {
                    return true; //next to existing room
                }
            }
        }
        return false;
    }


}
