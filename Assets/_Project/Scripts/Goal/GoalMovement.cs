using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalMovement : MonoBehaviour
{
    [Header("Goal movement settings")]

    [Tooltip("Array of way points for goal")]
    [SerializeField] Transform[] pointsTransform;

    [Tooltip("Goal movement speed")]
    [SerializeField] float speed = 20f;

    [Tooltip("Change target distance threshold")]
    [SerializeField] float positionThreshold = 0.3f; 

    private int currentPointIndex = 0; // Индекс текущей целевой точки
    private Transform goalTransform; //Кешированный трансформ ворот 

    void Start()
    {
        goalTransform = transform;
    }


    void Update()
    {
        MoveGoals();
    }

    void MoveGoals()
    {
        if (pointsTransform.Length == 0) return;

        Transform targetPoint = pointsTransform[currentPointIndex];

        goalTransform.position = Vector3.MoveTowards(goalTransform.position, targetPoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(goalTransform.position, targetPoint.position) <= positionThreshold)
        {
            currentPointIndex = (currentPointIndex + 1) % pointsTransform.Length;
        }
    }
}
