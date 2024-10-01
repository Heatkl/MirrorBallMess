using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    [SerializeField] private float maxForwardAngle = 45f; // Максимальный угол наклона вперёд
    [SerializeField] private float minBackwardAngle = -90f; // Минимальный угол отклонения назад
    [SerializeField] private float maxLeftAngle = 90f; // Максимальный угол поворота влево
    [SerializeField] private float maxRightAngle = 90f; // Максимальный угол поворота вправо
    [SerializeField] private GameObject ballProjectile;
    [SerializeField] private Transform ballSpawnPoint;
    [SerializeField] private Transform verticalRotationOrigin;
    [SerializeField] private Transform horizontalRotationOrigin;
    [SerializeField] private float ballShotForce;

    void Update()
    {

        verticalRotationOrigin.Rotate(0, 0, Input.GetAxis("Mouse X"));
        horizontalRotationOrigin.Rotate(0, Input.GetAxis("Mouse Y"), 0);

        if (Input.GetMouseButtonDown(0))
        {
            var ball = Instantiate(ballProjectile, ballSpawnPoint.position, Quaternion.identity);
            ball.transform.parent = null;
            ball.GetComponent<Rigidbody>().AddForce(ballSpawnPoint.forward * ballShotForce);
        }
           
    }
}
