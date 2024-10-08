using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    public void StopGame()
    {
        if(NetworkServer.active && NetworkClient.isConnected)
        {
            BallGameNetworkRoomManager.singleton.StopHost();
        }
        else if(NetworkClient.isConnected)
        {
            BallGameNetworkRoomManager.singleton.StopClient();
        }
        else if (NetworkServer.active)
        {
            BallGameNetworkRoomManager.singleton.StopServer();
        }
        else
        {
            Debug.Log("No available options to stop the game");
        }
    }
}
