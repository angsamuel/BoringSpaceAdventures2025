using UnityEngine;
using UnityEngine.UI;

public class UIWindow : MonoBehaviour
{

    Canvas ourCanvas;
    void Awake(){
        ourCanvas = GetComponent<Canvas>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show(){

        Debug.Log("SHOW");
        ourCanvas.enabled = true;
    }
    public void Hide(){
        Debug.Log("HIDE");
        ourCanvas.enabled = false;
    }
}
