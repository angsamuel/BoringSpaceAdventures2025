using UnityEngine;

public class SmoothDampFollow : MonoBehaviour
{
    public float smoothTime = 0.2f;
    private Vector3 velocity = Vector3.zero;

    private Transform parentTransform;

    void Start()
    {
        parentTransform = transform.parent;
    }

    void LateUpdate()
    {
        if (parentTransform == null) return;

        transform.position = Vector3.SmoothDamp(transform.position, parentTransform.position, ref velocity, smoothTime);
    }
}