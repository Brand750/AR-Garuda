using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [Tooltip("The speed of rotation in degrees per second.")]
    public float rotationSpeed = 15f;

    [Tooltip("The axis around which the object will rotate. " +
             "Vector3.up (0,1,0) for spinning like a top.")]
    public Vector3 rotationAxis = Vector3.up;

    void Update()
    {
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }
}