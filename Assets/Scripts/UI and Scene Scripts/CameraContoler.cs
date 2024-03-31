using UnityEngine;

public class CameraContoler : MonoBehaviour
{

    public float dampTime = 0.2f;

    public Transform target;

    private Vector3 moveVelocity;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 desiredPosition = target.position;

        desiredPosition.y = transform.position.y;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref moveVelocity, dampTime);   
    }


}
