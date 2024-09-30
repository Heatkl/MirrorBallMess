using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysicsTest : MonoBehaviour
{
    [Header("Controls of ball's hitting: WASD")]
    [Header("Ball hitting Force")]
    [SerializeField] float force;
    [Header("Ball Rigidbody")]
    [SerializeField] Rigidbody rb;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            rb.AddForce((/*Vector3.up +*/ Vector3.forward) * force);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            rb.AddForce((/*Vector3.up*/ - Vector3.forward) * force);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            rb.AddForce((/*Vector3.up */- Vector3.right) * force);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rb.AddForce((/*Vector3.up +*/ Vector3.right) * force);
        }

    }
}
