using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugPanel : MonoBehaviour
{

    public TextMeshProUGUI infoText;
    public Creature playerCreature;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        infoText.text = "Oxygen: " + playerCreature.GetOxygen().ToString("F2");
    }
}
