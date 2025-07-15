using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
public class GruntAI : MonoBehaviour
{
    [Header("Debug")]
    public string debugState;

    [Header("Config")]
    public float maxWanderDistance = 5;
    public float minApproachDistance = .25f;
    public float sightDistance = 10f;
    public LayerMask sightMask;

    [Header("References")]
    public Creature targetCreature;
    Creature myCreature;
    IEnumerator currentState;
    OxygenRefillStation oxyStation;

    //Trackers
    Vector3 lastTargetPos = Vector3.zero;






    void Awake(){
        myCreature = GetComponent<Creature>();

        returnPosition = transform.position;

        //oxyStation = GameObject.FindGameObjectWithTag("OxygenRefillStation").GetComponent<OxygenRefillStation>();
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

    public bool CanSeeTarget(){
        if(targetCreature == null){
            return false;
        }

        if(!CanSee(transform.position, targetCreature.transform.position)){
            targetCreature = null;
            return false;
        }

        return true;
    }
    IEnumerator PursuitState(){
        debugState = "PursuitState";

        yield return new WaitForSeconds(1f);

        NavMeshPath path = new NavMeshPath();
        int nextElement = 1;
        bool pathFound = GetComponent<NavMeshAgent>().CalculatePath(lastTargetPos, path);
        //GetComponent<NavMeshAgent>().enabled = false;
        if (!pathFound)
        {
            Debug.Log("No path found");
            ChangeState(WanderState());
            yield break;
        }

        Debug.Log("Path found");

        while (nextElement < path.corners.Length)
        {
            myCreature.MoveToward(path.corners[nextElement]);
            if (AttemptCombatState())
            {
                yield break;
            }

            float threshold = Mathf.Max(myCreature.GetPerFrameDistance(), minApproachDistance);

            if (Vector3.Distance(transform.position, path.corners[nextElement]) <= threshold)
            {
                nextElement++;
            }
            yield return null;

        }

        yield return null;
        if(AttemptCombatState()){
            yield break;
        }
        ChangeState(WanderState());
     }
    // IEnumerator PursuitStateLegacy(){
    //     Debug.Log("PursuitState");
    //     debugState = "PursuitState";

    //     while(Vector3.Distance(transform.position, lastTargetPos) > minApproachDistance)
    //     {
    //         yield return null;

    //         if (NeedOxygenRefill())
    //         {
    //             ChangeState(RefillOxygen());
    //             yield break;
    //         }

    //         if (AttemptCombatState())
    //         {
    //             yield break;
    //         }

    //         myCreature.MoveToward(lastTargetPos);

    //     }

    //     ChangeState(ReturnState());
    //     yield break;

    // }

    bool AttemptCombatState(){
        ChooseTarget();
        if(CanSeeTarget()){
            ChangeState(BlastState());
            return true;
        }
        return false;
    }

    IEnumerator WanderState(){
        Debug.Log("WanderState");

        debugState = "WanderState";
        yield return new WaitForSeconds(1f);

        Vector3 startPos = transform.position;
        returnPosition = transform.position;

        while(true){
            Vector3 wanderPositon = startPos + new Vector3(Random.Range(-maxWanderDistance, maxWanderDistance),0,Random.Range(-maxWanderDistance, maxWanderDistance));

            float timer = 0;
            while(timer < 1 && Vector3.Distance(transform.position,wanderPositon) > minApproachDistance){
                timer += Time.deltaTime;
                myCreature.MoveToward(wanderPositon);
                yield return null;
                if(AttemptCombatState()){
                    yield break;
                }
            }

            //yield return new WaitForSeconds(1f);

             timer = 0;
            while(timer < 1){
                yield return null;
                timer+=Time.deltaTime;
                if (AttemptCombatState())
                {
                    yield break;
                }
            }

            if(NeedOxygenRefill()){
                ChangeState(RefillOxygen());
                yield break;
            }

            //creature stops
        }

        yield return null;
    }

    bool NeedOxygenRefill(){
        if(myCreature.oxygen <= myCreature.maxOxygen / 2){
            return true;
        }
        return false;
    }

    IEnumerator RefillOxygen(){
        debugState = "RefillOxygen";
        OxygenRefillStation targetOxyStation = AIResourceManager.singleton.GetNearestOxygenRefillStation(transform.position);

        if(targetOxyStation == null){
            ChangeState(WanderState());
            yield break;
        }

        while(true){
            myCreature.MoveToward(targetOxyStation.transform.position);
            if (Vector3.Distance(transform.position, targetOxyStation.transform.position) < minApproachDistance)
            {
                ChangeState(ReturnState());
                yield break;
            }
            yield return null;
        }
        yield return null;
    }

    Vector3 returnPosition;
    IEnumerator ReturnState(){
        while(true){
            yield return null;
            if (AttemptCombatState())
            {
                yield break;
            }
            if (Vector3.Distance(transform.position, returnPosition) < minApproachDistance){
                ChangeState(WanderState());
                yield break;
            }
            myCreature.MoveToward(returnPosition);
        }
        yield return null;
    }


    void ChooseTarget(){
        List<Creature> creaturesInRange = AIResourceManager.singleton.GetCreaturesInRange(myCreature, transform.position, sightDistance);
        for(int i  = 0; i<creaturesInRange.Count; i++){
            if(CanSee(transform.position,creaturesInRange[i].transform.position)){
                targetCreature = creaturesInRange[i];
                Debug.Log(targetCreature.gameObject.name);
                return;
            }
        }
    }

    bool CanSee(Vector3 start, Vector3 destination){
        if(Vector3.Distance(start,destination) > sightDistance){
            return false;
        }
       if(!Physics.Linecast(start,destination,sightMask)){
         lastTargetPos = destination;
         return true;
       }
       return false;
    }

    IEnumerator BlastState(){

        while(CanSeeTarget()){
            myCreature.AimItemToward(targetCreature.transform.position);
            myCreature.UseItem();
            yield return null;
        }
        yield return null;

        ChangeState(PursuitState());
        yield break;
    }
}
