using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Lander : MonoBehaviour
{

    public Renderer canopyRenderer;
    public float canopyFadeTime = 5f;
    public Color opaqueCanopyColor;
    public Color transparentCanopyColor;


    //landing
    public float landTime = 2f;
    public Vector3 landingSkyPosition;
    public Vector3 landingGroundPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        StartCoroutine(LandRoutine(landingSkyPosition, landingGroundPosition));

    }



    // Update is called once per frame

    void Update()
    {
        // canopyFadeTime -= Time.deltaTime;
        // if(canopyFadeTime < 0){
        //     canopyFadeTime = 0;
        // }
        // canopyRenderer.material.color = new Color(1, 1, 1, Mathf.Clamp(canopyFadeTime,0,1));


    }


    IEnumerator FadeCanopy(Color startColor, Color endColor){
        float timer = 0;
        while(timer <= 1){
            timer += Time.deltaTime / canopyFadeTime;
            canopyRenderer.material.color = Color.Lerp(startColor, endColor,timer);
            yield return null;
        }
        canopyRenderer.material.color = endColor;
        yield return null;
    }

    IEnumerator LandRoutine(Vector3 startPosition, Vector3 endPosition){
        float timer = 0;
        while(timer <= 1){
            timer += Time.deltaTime / landTime;
            transform.position = Vector3.Lerp(startPosition,endPosition,timer);
            yield return null;
        }
        transform.position = endPosition;
        StartCoroutine(FadeCanopy(opaqueCanopyColor, transparentCanopyColor));
        yield return null;
    }

    IEnumerator TakeOffRoutine(Vector3 startPosition, Vector3 endPosition){
        StartCoroutine(FadeCanopy(transparentCanopyColor, opaqueCanopyColor));
        yield return new WaitForSeconds(canopyFadeTime);
        float timer = 0;
        while(timer <= 1){
            timer += Time.deltaTime / landTime;
            transform.position = Vector3.Lerp(startPosition,endPosition,timer);
            yield return null;
        }
        transform.position = endPosition;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        yield return null;
    }

    public void TakeOff(){
        StartCoroutine(TakeOffRoutine(landingGroundPosition, landingSkyPosition));
    }

    // IEnumerator FadeInCanopy()
    // {
    //     float timer = 0;
    //     while (timer <= 1)
    //     {
    //         timer += Time.deltaTime / canopyFadeTime;
    //         canopyRenderer.material.color = new Color(1, 1, 1, timer);
    //         yield return null;
    //     }
    //     canopyRenderer.material.color = new Color(1, 1, 1, 0);
    //     yield return null;
    // }


}
