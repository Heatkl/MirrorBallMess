using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class BallSpawner : NetworkBehaviour
{
    [Header("Spawn features")]

    [Tooltip("Ball prefab")]
    public GameObject ballProjectile;

    [Tooltip("Point of ball spawn for the player")]
    public Transform ballSpawnPoint;        

    [SyncVar]
    public string ballTag;                                                          // ��� ������ ��� ����, ������������������ �� ���� ��������

    private float maxBallLifeTime = 120f;                                           // ����� ����� ���� �� �����
    private int maxBalls = 1000;                                                    // ������������ ���������� ����� �� �����

    private SyncList<GameObject> availableBallPool = new SyncList<GameObject>();    // ��� ��������� �����
    private Queue<GameObject> activeBallsQueue = new Queue<GameObject>();           // ������� �������� �����

    private int currentBallCount = 0;                                               // ������� ���������� ����� �� �����
    private bool canPoolBall = false;                                               // ����, ����������� �� ������� ����� � ����
    private float ballShotForce = 100000f;                                         //���� ���� ��� ������ �������� ���������� ������

    /// <summary>
    /// ����� �������� ���� �� ������� �� �������� ������
    /// </summary>
    public void SpawnBall()
    {
        CmdSpawnBall(gameObject.tag);
    }


    /// <summary>
    /// ����� ���� ��������� �� ������� � ��������� ����� ������
    /// </summary>
    /// <param name="tag">���, ������� ����� � ����, ��� ������������� � ���������� �����</param>
    [Command]
    private void CmdSpawnBall(string tag)
    {
        canPoolBall = availableBallPool.Count > 0;
        if (currentBallCount >= maxBalls && !canPoolBall)
        {
            // ���� ����� ���� �������� ������������� �����, � � ���� ��� ��������� �����, �������� ����� ������ ������������ ���
            GameObject oldestBall = activeBallsQueue.Dequeue();
            oldestBall.SetActive(false);
            RpcDeactivateBallOnClients(oldestBall);
            availableBallPool.Add(oldestBall);
            currentBallCount--; 
        }

        GameObject ball;

        // ���� ���� ���� � ����, ���������� ���� �� ���
        if (canPoolBall)
        {
            ball = availableBallPool[0];
            availableBallPool.RemoveAt(0);
            ball.SetActive(true);
            RpcActivateBallOnClients(ball);
            ball.transform.position = ballSpawnPoint.position;
            ball.transform.rotation = ballSpawnPoint.rotation;
        }
        else
        {
            // ������ ����� ���, ���� �� �������� �����, �� � ���� ��� �����
            ball = Instantiate(ballProjectile, ballSpawnPoint.position, ballSpawnPoint.rotation);
            NetworkServer.Spawn(ball); 
            currentBallCount++;
        }

        // ����������� ���� ��� ������, � ��������� ���������� �������� � ���� ����� ���������
        ball.tag = tag;
        var rb = ball.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.AddForce(ballSpawnPoint.forward * ballShotForce);

        // ��������� ��� � ������� �������� �����
        activeBallsQueue.Enqueue(ball);

        // ��������� ������ �� ����������� ����
        StartCoroutine(DeactivateBallAfterTime(ball, maxBallLifeTime));
    }

   /// <summary>
   /// ������������ ��� �� ��������� ������� ����� ����
   /// </summary>
   /// <param name="ball">���, ������� ��� ���������</param>
   /// <param name="time">����� ����� ����</param>
   /// <returns></returns>
    private IEnumerator DeactivateBallAfterTime(GameObject ball, float time)
    {
        yield return new WaitForSeconds(time);

        // ������������ ��� � ���������� ��������
        ball.SetActive(false);
        RpcDeactivateBallOnClients(ball);

        // ��������� ��� � ��� ��������� ����� � ������� ��� �� ������� �������� �����
        availableBallPool.Add(ball);
        if(activeBallsQueue.Count>0)
        activeBallsQueue.Dequeue();
    }


    /// <summary>
    /// ������������ ��� �� ���� ��������
    /// </summary>
    /// <param name="ball">���, ������� ��������� ��������������</param>
    [ClientRpc]
    private void RpcDeactivateBallOnClients(GameObject ball)
    {
        if (ball != null)
        {
            ball.SetActive(false);
        }
    }


    /// <summary>
    /// ��������� ���� �� ���� ��� ���� ��������
    /// </summary>
    /// <param name="ball">���, ������� ��������� ������������</param>
    [ClientRpc]
    private void RpcActivateBallOnClients(GameObject ball)
    {
        if (ball != null)
        {
            ball.SetActive(true);
        }
    }
}