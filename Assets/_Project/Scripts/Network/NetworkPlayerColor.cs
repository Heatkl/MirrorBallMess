using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerColor : NetworkBehaviour
{

    [SyncVar(hook = nameof(OnColorChanged))]
    private Color playerColor; // ������ ���� ������, ���������������� � ���������

    [SyncVar(hook = nameof(OnColorTagChanged))]
    private string playerColorTag; // ������ ��� ����� ������, ���������������� � ���������

    [Header("Player objects to change")]

    [Tooltip("All object's renderers with changed player color")]
    [SerializeField] private MeshRenderer[] playerColorRenderers;

    [Tooltip("All objects with changed tag")]
    [SerializeField] private GameObject[] playerTagObjects;


    /// <summary>
    /// ��������� ����� �� ������� � ������������� � ���������
    /// </summary>
    /// <param name="color">���� ������</param>
    [Command]
    private void CmdSetPlayerColor(Color color)
    {
        playerColor = color; 
    }

    /// <summary>
    /// ��������� ���� ������ �� ������� ��� ������������� � ���������
    /// </summary>
    /// <param name="colorTag">��� ������</param>
    [Command]
    private void CmdSetPlayerColorTag(string colorTag)
    {
        playerColorTag = colorTag; 
    }

    /// <summary>
    /// ������������� ������ ���������������� ����� �� ������� �� ����
    /// </summary>
    /// <param name="colorTag">��� ������</param>
    [Command]
    private void CmdSetPlayerColorScore(string colorTag)
    {
            ScoreManager.Instance.CmdAddPlayerScore(colorTag);
    }

    /// <summary>
    /// Hook ��� ��������� ����� ������ ��� ��������������
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
    /// ��� ��� ��������� ���� �� ������
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
    /// �������� �� ��������� ���������������� � ������������� �� ������.
    /// ��� � �� �������� ������ �������� �� Mirror
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
