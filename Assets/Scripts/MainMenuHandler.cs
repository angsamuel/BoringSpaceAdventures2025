using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public void PlayGame(){
        Debug.Log("Play Game!");
        SceneManager.LoadScene("Mission");
    }
}
