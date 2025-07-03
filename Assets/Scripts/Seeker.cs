using UnityEngine;

public class Seeker : MonoBehaviour
{

    public Transform targetTransform;
    public float speed = 10f;
    public float minDistance = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(targetTransform.position,transform.position) < minDistance){
            return;
        }


        Vector3 seekerMovement = targetTransform.position - transform.position;
        transform.position = transform.position + (seekerMovement.normalized * speed * Time.deltaTime );
    }
}
