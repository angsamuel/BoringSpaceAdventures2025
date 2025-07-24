using UnityEngine;

public class AimReticle : MonoBehaviour
{
    public Blaster blaster;

    public RectTransform top;
    public RectTransform bottom;
    public RectTransform left;
    public RectTransform right;

    public float defaultSpread = 14f;

    public float reticleMaxDistance = 50f;


    void Update(){
        if(blaster.downSightsProgress > 0){
            top.gameObject.SetActive(false);
            bottom.gameObject.SetActive(false);
            left.gameObject.SetActive(false);
            right.gameObject.SetActive(false);
            return;
        }

        top.gameObject.SetActive(true);
        bottom.gameObject.SetActive(true);
        left.gameObject.SetActive(true);
        right.gameObject.SetActive(true);

        float spreadAmount = Mathf.Lerp(0, reticleMaxDistance, blaster.currentSpread);
        top.anchoredPosition = new Vector2(0, spreadAmount);
        bottom.anchoredPosition = new Vector2(0, -spreadAmount);
        left.anchoredPosition = new Vector2(-spreadAmount, 0);
        right.anchoredPosition = new Vector2(spreadAmount, 0);
    }
}
