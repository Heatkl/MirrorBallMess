using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LobbyNameManager : MonoBehaviour
{
    private Dictionary<string, int> namePortPairs = new Dictionary<string, int>();
    [SerializeField] private int[] ports; 

    public int GetLobbyPort(string lobbyName)
    {
        if (namePortPairs.ContainsKey(lobbyName))
        {
            return namePortPairs[lobbyName];
        }
        else
        {
            return -1;
        }
    }

    public string[] GetAllLobbyNames()
    {
        List<string> names = new List<string>();
        foreach(string name in namePortPairs.Keys)
        {
            names.Add(name);
        }

        return names.ToArray();
    }

    public void AddNewLobby(string lobbyName)
    {
        int lobbyPort = GetFreePort();
        namePortPairs.Add(lobbyName, lobbyPort);
    }

    private int GetFreePort()
    {
        throw new NotImplementedException();
    }
}
