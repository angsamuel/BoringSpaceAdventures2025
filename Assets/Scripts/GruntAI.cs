using UnityEngine;
using System.Collections;
public class GruntAI : MonoBehaviour
{
    [Header("Debug")]
    public string debugState;

    [Header("Config")]
    public float maxWanderDistance = 5;
    public float minApproachDistance = .25f;
    public float sightDistance = 10f;

    [Header("References")]
    public Transform targetTransform;
    Creature myCreature;
    IEnumerator currentState;

    void Awake(){
        myCreature = GetComponent<Creature>();
    }
    void Start(){
        ChangeState(WanderState());
    }
    void ChangeState(IEnumerator newState){

        if(currentState != null){
            StopCoroutine(currentState);
        }
        StartCoroutine(newState);
    }

    public bool HasTarget(){
        return Vector3.Distance(transform.position, targetTransform.position) < sightDistance;
    }

    IEnumerator PursuitState(){
        Debug.Log("PursuitState");
        debugState = "PursuitState";

        while(Vector3.Distance(transform.position, targetTransform.position) <= sightDistance)
        {
            yield return null;
            if(Vector3.Distance(transform.position, targetTransform.position) > minApproachDistance){
                myCreature.MoveToward(targetTransform.position);
            }
        }

        ChangeState(WanderState());
        yield break;

    }

    IEnumerator WanderState(){
        Debug.Log("WanderState");

        debugState = "WanderState";
        yield return new WaitForSeconds(1f);

        Vector3 startPos = transform.position;

        while(true){
            Vector3 wanderPositon = startPos + new Vector3(Random.Range(-maxWanderDistance, maxWanderDistance),0,Random.Range(-maxWanderDistance, maxWanderDistance));

            float timer = 0;
            while(timer < 1 && Vector3.Distance(transform.position,wanderPositon) > minApproachDistance){
                timer += Time.deltaTime;
                myCreature.MoveToward(wanderPositon);
                yield return null;
                if(HasTarget()){
                    ChangeState(PursuitState());
                    yield break;
                }
            }


            yield return new WaitForSeconds(1f);

            //creature stops
        }

        yield return null;
    }
}
