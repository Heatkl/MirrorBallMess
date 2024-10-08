using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : NetworkBehaviour
{
    [Header("Cannon rotation settings")]

    [Tooltip("Maximum forward rotation")]
    [SerializeField] private float maxForwardAngle = 45f;

    [Tooltip("Maximum backward rotation")]
    [SerializeField] private float minBackwardAngle = -90f;

    [Tooltip("Maximum cannon rotation to left side")]
    [SerializeField] private float maxLeftAngle = 90f;

    [Tooltip("Maximum cannon rotation to right side")]
    [SerializeField] private float maxRightAngle = 90f;

    [Tooltip("Transform for vertical rotation")]
    [SerializeField] private Transform verticalCannonRotationOrigin;

    [Tooltip("Transform for horizontal rotation")]
    [SerializeField] private Transform horizontalCannonRotationOrigin;

    [Tooltip("Ball spawner on player")]
    [SerializeField] BallSpawner ballSpawner;

    private float mouseMovementSpeed = 0.5f;//Коэффициент для вращения пушки



    public override void OnStartLocalPlayer()
    {
        ballSpawner = gameObject.GetComponent<BallSpawner>();
    }


    /// <summary>
    /// Метод для вызова из Input Actions для выстрела мячом
    /// </summary>
    public void SpawnBall()
    {
        if (!isLocalPlayer)
            return;

        ballSpawner.SpawnBall();
    }

    /// <summary>
    /// Метод для вызова из Input Actions для вращения пушки с рассчетом углов
    /// </summary>
    /// <param name="mouseMove">Дельта перемещение мыши</param>
    public void RotateCannon(Vector2 mouseMove)
    {
        if (!isLocalPlayer)
            return;

        mouseMove = mouseMove.normalized * mouseMovementSpeed;


        Vector3 currentVerticalRotation = verticalCannonRotationOrigin.localEulerAngles;
        Vector3 currentHorizontalRotation = horizontalCannonRotationOrigin.localEulerAngles;


        if (currentVerticalRotation.y > 180) currentVerticalRotation.y -= 360;
        if (currentHorizontalRotation.y > 180) currentHorizontalRotation.y -= 360;


        float newVerticalRotationY = Mathf.Clamp(currentVerticalRotation.y + mouseMove.y, minBackwardAngle, maxForwardAngle);
        float newHorizontalRotationY = Mathf.Clamp(currentHorizontalRotation.y + mouseMove.x, -maxLeftAngle, maxRightAngle);


        verticalCannonRotationOrigin.localEulerAngles = new Vector3(currentVerticalRotation.x, newVerticalRotationY, currentVerticalRotation.z);
        horizontalCannonRotationOrigin.localEulerAngles = new Vector3(currentHorizontalRotation.x, newHorizontalRotationY, currentHorizontalRotation.z);

    }

}


