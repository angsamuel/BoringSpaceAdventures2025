using UnityEngine;

public class Beacon : MonoBehaviour
{

    public Renderer lightRenderer;
    public Color lightOffColor = Color.red;
    public Color lightOnColor = Color.green;

    bool isOn = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lightRenderer.material.color = lightOffColor;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TurnOn(){
        if(isOn){
            return;
        }


        GetComponent<AudioSource>().Play();

        lightRenderer.material.color = lightOnColor;
        isOn = true;
    }

    public bool IsOn(){
        return isOn;
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            TurnOn();
        }
    }
}
