using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SwapDungeonGenerator : MonoBehaviour
{

    public bool needsRegen = true;
    public int seed = 0;
    public List<GameObject> piecePrefabs;
    List<GameObject> pieces;
    HashSet<Vector3> visited = new HashSet<Vector3>();


    [Header("Config")]
    // public int width = 5;
    // public int height = 5;
    public int steps = 10;


    public float noPieceChance = 0.5f;
    void Awake(){

        pieces = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(needsRegen){
            ClearDungeon();
            StartCoroutine(GenerateWalkRoutine());
            needsRegen = false;
        }
    }

    public void ClearDungeon(){

        foreach(GameObject piece in pieces){
            Destroy(piece);
        }
        pieces.Clear();
    }

    public IEnumerator GenerateWalkRoutine(){
        yield return null;
        Random.InitState(seed);
        Vector3 position = new Vector3(0,0,0);

        Vector3 north = new Vector3(0,0,1);
        Vector3 south = new Vector3(0, 0, -1);
        Vector3 east = new Vector3(1,0,0);
        Vector3 west = new Vector3(-1,0,0);

        for(int i = 0; i<steps; i++){

            yield return new WaitForSeconds(.1f);

            if(!visited.Contains(position) && Random.Range(0f, 1f) > noPieceChance)
            {
                GameObject newPiece = Instantiate(piecePrefabs[Random.Range(0,piecePrefabs.Count)], position,Quaternion.identity);
                pieces.Add(newPiece);
                visited.Add(position);
            }
            int randomDirection = Random.Range(0,4);
                switch(randomDirection){
                    case 0:
                        position += north;
                        break;
                    case 1:
                        position += south;
                        break;
                    case 2:
                        position += east;
                        break;
                    case 3:
                        position += west;
                        break;
                }
            }
        }

    // public void GenerateGrid(){
    //     pieces = new List<GameObject>();


    //     Random.InitState(seed);
    //     for(int x = 0; x<width;x++){
    //         for(int z = 0; z<height; z++){
    //             if (Random.Range(0f, 1f) < noPieceChance)
    //             {
    //                 continue;
    //             }

    //             GameObject randomPrefab = piecePrefabs[Random.Range(0, piecePrefabs.Count)];

    //             GameObject newPiece =Instantiate(randomPrefab, new Vector3(x,0,z), Quaternion.identity);
    //             pieces.Add(newPiece);


    //         }
    //     }
    // }
}
