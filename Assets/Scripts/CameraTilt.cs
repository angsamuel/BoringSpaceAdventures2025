using UnityEngine;

public class CameraTilt : MonoBehaviour
{

    public float maxAngle;
    public float currentAngle = 0;
    public float tiltSpeed = 1f;
    public float tiltDecay = .9f;
    public void Tilt(float moveX){

        currentAngle -= moveX * tiltSpeed * Time.deltaTime;
        currentAngle = Mathf.Clamp(currentAngle,-maxAngle,maxAngle);

        if (moveX == 0)
        {
            currentAngle *= 1 - (tiltDecay * Time.deltaTime);
        }

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,transform.localEulerAngles.y, currentAngle);
    }
}
