using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GoalHitting : NetworkBehaviour
{
    private int ballIncreasePoints = 10;//Количество очков за попадание мячом по вражеским воротам
    private int goalDecreasePoints = -5;//Количество очков за попадание мяча в свои ворота



    private void OnCollisionEnter(Collision collision)
    {

        string ballTag = collision.gameObject.tag;

        if (gameObject.CompareTag(ballTag))
        {
            Debug.Log("Player hit himself!");
            if(isServer)
            RpcGlobalScore(goalDecreasePoints);
        }
        else 
        {
            Debug.Log("Player hit by another player!");
            if (isServer)
            {
                RpcGlobalScore(goalDecreasePoints);
                RpcGlobalScore(ballTag, ballIncreasePoints);
            }

        }


    }

    [ClientRpc]
    private void RpcGlobalScore(int score)
    {
        switch (gameObject.tag)
        {
            case "Orange":
                ScoreManager.Instance.orangeScore += score;
                break;
            case "Blue":
                ScoreManager.Instance.blueScore += score;
                break;
            case "Red":
                ScoreManager.Instance.redScore += score;
                break;
            case "Green":
                ScoreManager.Instance.greenScore += score;
                break;
        }
    }

    [ClientRpc]
    private void RpcGlobalScore(string tag, int score)
    {
        switch (tag)
        {
            case "Orange":
                ScoreManager.Instance.orangeScore += score;
                break;
            case "Blue":
                ScoreManager.Instance.blueScore += score;
                break;
            case "Red":
                ScoreManager.Instance.redScore += score;
                break;
            case "Green":
                ScoreManager.Instance.greenScore += score;
                break;
        }
    }
}
