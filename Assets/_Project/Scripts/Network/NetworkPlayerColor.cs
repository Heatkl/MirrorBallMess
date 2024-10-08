using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerColor : NetworkBehaviour
{

    [SyncVar(hook = nameof(OnColorChanged))]
    private Color playerColor; // Хранит цвет игрока, синхронизируется с клиентами

    [SyncVar(hook = nameof(OnColorTagChanged))]
    private string playerColorTag; // Хранит тег цвета игрока, синхронизируется с клиентами

    [Header("Player objects to change")]

    [Tooltip("All object's renderers with changed player color")]
    [SerializeField] private MeshRenderer[] playerColorRenderers;

    [Tooltip("All objects with changed tag")]
    [SerializeField] private GameObject[] playerTagObjects;


    /// <summary>
    /// Установка цвета на сервере и синхронизация с клиентами
    /// </summary>
    /// <param name="color">Цвет игрока</param>
    [Command]
    private void CmdSetPlayerColor(Color color)
    {
        playerColor = color; 
    }

    /// <summary>
    /// Установка тега игрока на сервере для синхронизации с клиентами
    /// </summary>
    /// <param name="colorTag">Тег игрока</param>
    [Command]
    private void CmdSetPlayerColorTag(string colorTag)
    {
        playerColorTag = colorTag; 
    }

    /// <summary>
    /// Инициализация игрока соответствующего цвета на сервере по тегу
    /// </summary>
    /// <param name="colorTag">Тег игрока</param>
    [Command]
    private void CmdSetPlayerColorScore(string colorTag)
    {
            ScoreManager.Instance.CmdAddPlayerScore(colorTag);
    }

    /// <summary>
    /// Hook при изменении цвета игрока для перекрашивания
    /// </summary>
    /// <param name="oldColor"></param>
    /// <param name="newColor"></param>
    private void OnColorChanged(Color oldColor, Color newColor)
    {
        foreach (var renderer in playerColorRenderers)
        {
            renderer.material.color = newColor;
        }
    }

    /// <summary>
    /// Хук для изменения тега на игроке
    /// </summary>
    /// <param name="oldColorTag"></param>
    /// <param name="newColorTag"></param>
    private void OnColorTagChanged(string oldColorTag, string newColorTag)
    {
        foreach(var tagObject  in playerTagObjects)
        {
            tagObject.tag = newColorTag;
        }
    }

    
    public override void OnStartLocalPlayer()
    {

        if (isLocalPlayer)
        {
            StartCoroutine(WaitingOneSecond());  
        }
    }

    /// <summary>
    /// Задержка во избежание рассинхронизации в инициализации на старте.
    /// Так и не подобрал нужных колбеков от Mirror
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitingOneSecond()
    {
        yield return new WaitForSeconds(1f);
        Color _playerColor = BallGameNetworkRoomManager.singleton.PlayerColor;
        string _playerColorTag = BallGameNetworkRoomManager.singleton.PlayerColorTag;
        CmdSetPlayerColor(_playerColor);
        CmdSetPlayerColorTag(_playerColorTag);
        CmdSetPlayerColorScore(_playerColorTag);

    }
}
