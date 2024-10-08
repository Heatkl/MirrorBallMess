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
    public string ballTag;                                                          // “ег игрока дл€ м€ча, синхронизированный на всех клиентах

    private float maxBallLifeTime = 120f;                                           // ¬рем€ жизни м€ча на сцене
    private int maxBalls = 1000;                                                    // ћаксимальное количество м€чей на сцене

    private SyncList<GameObject> availableBallPool = new SyncList<GameObject>();    // ѕул доступных м€чей
    private Queue<GameObject> activeBallsQueue = new Queue<GameObject>();           // ќчередь активных м€чей

    private int currentBallCount = 0;                                               // “екущее количество м€чей на сцене
    private bool canPoolBall = false;                                               // ‘лаг, указывающий на наличие м€чей в пуле
    private float ballShotForce = 100000f;                                         //—ила м€ча дл€ броска согласно настройкам физики

    /// <summary>
    /// ¬ызов спавнера м€ча на сервере из внешнего класса
    /// </summary>
    public void SpawnBall()
    {
        CmdSpawnBall(gameObject.tag);
    }


    /// <summary>
    /// —павн м€ча клиентами на сервере с указанным тегом игрока
    /// </summary>
    /// <param name="tag">“ег, который будет у м€ча, дл€ идентификации и начислени€ очков</param>
    [Command]
    private void CmdSpawnBall(string tag)
    {
        canPoolBall = availableBallPool.Count > 0;
        if (currentBallCount >= maxBalls && !canPoolBall)
        {
            // ≈сли спавн м€ча превысил установленный лимит, а в пуле нет доступных м€чей, забираем самый ранний заспавненный м€ч
            GameObject oldestBall = activeBallsQueue.Dequeue();
            oldestBall.SetActive(false);
            RpcDeactivateBallOnClients(oldestBall);
            availableBallPool.Add(oldestBall);
            currentBallCount--; 
        }

        GameObject ball;

        // ≈сли есть м€чи в пуле, активируем один из них
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
            // —оздаЄм новый м€ч, если не превышен лимит, но в пуле нет м€чей
            ball = Instantiate(ballProjectile, ballSpawnPoint.position, ballSpawnPoint.rotation);
            NetworkServer.Spawn(ball); 
            currentBallCount++;
        }

        // ѕрисваиваем м€чу тег игрока, и скидываем физическую скорость в ноль перед выстрелом
        ball.tag = tag;
        var rb = ball.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.AddForce(ballSpawnPoint.forward * ballShotForce);

        // ƒобавл€ем м€ч в очередь активных м€чей
        activeBallsQueue.Enqueue(ball);

        // «апускаем таймер на деактивацию м€ча
        StartCoroutine(DeactivateBallAfterTime(ball, maxBallLifeTime));
    }

   /// <summary>
   /// ƒеактивирует м€ч по заданному времени жизни м€ча
   /// </summary>
   /// <param name="ball">ћ€ч, который был заспавнен</param>
   /// <param name="time">¬рем€ жизни м€ча</param>
   /// <returns></returns>
    private IEnumerator DeactivateBallAfterTime(GameObject ball, float time)
    {
        yield return new WaitForSeconds(time);

        // ƒеактивируем м€ч и уведомл€ем клиентов
        ball.SetActive(false);
        RpcDeactivateBallOnClients(ball);

        // ƒобавл€ем м€ч в пул доступных м€чей и убираем его из очереди активных м€чей
        availableBallPool.Add(ball);
        if(activeBallsQueue.Count>0)
        activeBallsQueue.Dequeue();
    }


    /// <summary>
    /// ƒеактивируем м€ч на всех клиентах
    /// </summary>
    /// <param name="ball">ћ€ч, который требуетс€ деактивировать</param>
    [ClientRpc]
    private void RpcDeactivateBallOnClients(GameObject ball)
    {
        if (ball != null)
        {
            ball.SetActive(false);
        }
    }


    /// <summary>
    /// јктиваци€ м€ча из пула дл€ всех клиентов
    /// </summary>
    /// <param name="ball">ћ€ч, который требуетс€ активировать</param>
    [ClientRpc]
    private void RpcActivateBallOnClients(GameObject ball)
    {
        if (ball != null)
        {
            ball.SetActive(true);
        }
    }
}