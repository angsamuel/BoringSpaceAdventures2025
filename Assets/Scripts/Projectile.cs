using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 0;

    public void SetDamage(int newDamage){
        damage = newDamage;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            //Instantiate(poof, transform.position, Quaternion.identity);
            Debug.Log("DESTROY");
            Destroy(gameObject);
        }
        else if (collision.gameObject.GetComponent<Creature>() != null)
        {
            collision.gameObject.GetComponent<Creature>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
