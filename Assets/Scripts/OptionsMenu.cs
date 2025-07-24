using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class OptionsMenu : MonoBehaviour
{

    public AudioMixer audioMixer;
    public Slider masterVolumeSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetMasterVolume(){
        SetVolume("MasterVolume",masterVolumeSlider.value);
    }

    void SetVolume(string name, float value){

        PlayerPrefs.SetFloat(name, value);
        float adjustedVolume = Mathf.Log10(value) * 20;
        if(value == 0){
            adjustedVolume = -80;
        }
        audioMixer.SetFloat(name, adjustedVolume);
    }
}
