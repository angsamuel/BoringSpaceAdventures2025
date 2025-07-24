using UnityEngine;

public class BuildingCross : MonoBehaviour
{
    public GameObject upWall;
    public GameObject downWall;
    public GameObject leftWall;
    public GameObject rightWall;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ChooseWalls(bool up, bool down, bool left, bool right){
        if(!up){
            Destroy(upWall);
        }
        if(!down){
            Destroy(downWall);
        }
        if(!left){
            Destroy(leftWall);
        }
        if(!right){
            Destroy(rightWall);
        }
    }

}
