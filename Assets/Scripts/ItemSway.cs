using UnityEngine;

public class ItemSway : MonoBehaviour
{
    public float swayIntensity;
    public float smoothFactor;
    void Update(){
        float mouseX = Input.GetAxisRaw("Mouse X") * swayIntensity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayIntensity;

        Quaternion qx = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion qy = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion qFinal = qx * qy;

        transform.localRotation = Quaternion.Slerp(transform.localRotation,qFinal, smoothFactor*Time.deltaTime);
    }
}
