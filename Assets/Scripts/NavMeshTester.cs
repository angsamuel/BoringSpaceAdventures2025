using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NavMeshTester : MonoBehaviour
{
    public Transform target;
    Creature myCreature;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myCreature = GetComponent<Creature>();
        // GetComponent<NavMeshAgent>().isStopped = false;
        // GetComponent<NavMeshAgent>().destination = target.position;
        StartCoroutine(FindPath());
    }

    // Update is called once per frame
    void Update()
    {
        //myCreature.MoveToward(transform.position + new Vector3(0,0,10));
    }

    IEnumerator FindPath(){
        NavMeshPath path = new NavMeshPath();
        int nextElement = 0;
        bool pathFound = GetComponent<NavMeshAgent>().CalculatePath(target.position,path);
        //GetComponent<NavMeshAgent>().enabled = false;
        if(!pathFound){
            Debug.Log("No path found");
            yield break;
        }

        Debug.Log("Path found");

        while(nextElement < path.corners.Length){

            if(Vector3.Distance(transform.position,path.corners[nextElement]) > 2f){
                yield return null;
                myCreature.MoveToward(path.corners[nextElement]);
            }else{
                nextElement++;
            }
        }
    }

}
