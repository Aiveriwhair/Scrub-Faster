using UnityEngine;

public class CircularCameraMovement : MonoBehaviour
{
    public Transform target;
    public float radius = 5f;
    public float speed = 2f;

    private void Update()
    {
        float angle = Time.time * speed;
        float x = target.position.x + Mathf.Cos(angle) * radius;
        float z = target.position.z + Mathf.Sin(angle) * radius;

        transform.position = new Vector3(x, transform.position.y, z);

        transform.LookAt(target.position);
    }
}