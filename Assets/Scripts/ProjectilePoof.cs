using UnityEngine;

public class ProjectilePoof : MonoBehaviour
{
    public GameObject poof;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("POOF");
            Destroy(Instantiate(poof, collision.transform.position, Quaternion.identity),1);
        }
    }
}
