using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScoreManager : NetworkBehaviour
{

    public static ScoreManager Instance;

    [HideInInspector]
    [SyncVar(hook = nameof(RpcOnOrangeScoreChanged))]
    public int orangeScore = 0;

    [HideInInspector]
    [SyncVar(hook = nameof(RpcOnBlueScoreChanged))]
    public int blueScore = 0;

    [HideInInspector]
    [SyncVar(hook = nameof(RpcOnGreenScoreChanged))]
    public int greenScore = 0;

    [HideInInspector]
    [SyncVar(hook = nameof(RpcOnRedScoreChanged))]
    public int redScore = 0;

    [Header("Player score text executors")]

    [SerializeField] FillScoreExecutor orangeScoreExecutors;
    [SerializeField] FillScoreExecutor blueScoreExecutors;
    [SerializeField] FillScoreExecutor greenScoreExecutors;
    [SerializeField] FillScoreExecutor redScoreExecutors;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }
    }


    [ClientRpc]
    public void CmdAddPlayerScore(string _playerTag)
    {
        switch (_playerTag)
        {
            case "Orange":
                orangeScoreExecutors.gameObject.SetActive(true);
                break;
            case "Blue":
                blueScoreExecutors.gameObject.SetActive(true);
                break;
            case "Red":
                redScoreExecutors.gameObject.SetActive(true);
                break;
            case "Green":
                greenScoreExecutors.gameObject.SetActive(true);                                                           
                break;
        }
    }

    [ClientRpc]
    void RpcOnOrangeScoreChanged(int oldScore, int newScore)
    {
        orangeScoreExecutors.UpdateScoreText(newScore);
    }

    [ClientRpc]
    void RpcOnBlueScoreChanged(int oldScore, int newScore)
    {
        blueScoreExecutors.UpdateScoreText(newScore);
    }
    [ClientRpc]
    void RpcOnRedScoreChanged(int oldScore, int newScore)
    {
        redScoreExecutors.UpdateScoreText(newScore);
    }
    [ClientRpc]
    void RpcOnGreenScoreChanged(int oldScore, int newScore)
    {
        greenScoreExecutors.UpdateScoreText(newScore);
    }
}


