using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[AddComponentMenu("")]
public class BallGameNetworkRoomManager : NetworkRoomManager
{

    // Хранит выбранный цвет игрока
    private Color selectedColor = Color.white;
    private Color[] colorOptions = { Color.red, new Color(1f, 0.64f, 0f), Color.green, Color.blue };
    private bool showColorDropdown = false;

    private int selectedColorIndex = -1;

    //Свойство для цвета игрока
    public Color PlayerColor
    {
        get { return selectedColor; }
    }

    //Свойство для определения тега по цвету
    public string PlayerColorTag
    {
        get
        {
            var tag = ((PlayerColorTags)selectedColorIndex).ToString();
            return tag;
        }
    }

    public static new BallGameNetworkRoomManager singleton => NetworkManager.singleton as BallGameNetworkRoomManager;

    /// <summary>
    /// This is called on the server when a networked scene finishes loading.
    /// </summary>
    /// <param name="sceneName">Name of the new scene.</param>
    public override void OnRoomServerSceneChanged(string sceneName)
    {
        // spawn the initial batch of Rewards
        //if (sceneName == GameplayScene)
        //    ;
        //Spawner.InitialSpawn();
    }

    /// <summary>
    /// Called just after GamePlayer object is instantiated and just before it replaces RoomPlayer object.
    /// This is the ideal point to pass any data like player name, credentials, tokens, colors, etc.
    /// into the GamePlayer object as it is about to enter the Online scene.
    /// </summary>
    /// <param name="roomPlayer"></param>
    /// <param name="gamePlayer"></param>
    /// <returns>true unless some code in here decides it needs to abort the replacement</returns>
    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        //PlayerScore playerScore = gamePlayer.GetComponent<PlayerScore>();
        //playerScore.index = roomPlayer.GetComponent<NetworkRoomPlayer>().index;
        return true;
    }

    public override void OnRoomStopClient()
    {
        base.OnRoomStopClient();
    }

    public override void OnRoomStopServer()
    {
        base.OnRoomStopServer();
    }

    /*
        This code below is to demonstrate how to do a Start button that only appears for the Host player
        showStartButton is a local bool that's needed because OnRoomServerPlayersReady is only fired when
        all players are ready, but if a player cancels their ready state there's no callback to set it back to false
        Therefore, allPlayersReady is used in combination with showStartButton to show/hide the Start button correctly.
        Setting showStartButton false when the button is pressed hides it in the game scene since NetworkRoomManager
        is set as DontDestroyOnLoad = true.
    */

    bool showStartButton;

    public override void OnRoomServerPlayersReady()
    {
        // calling the base method calls ServerChangeScene as soon as all players are in Ready state.
        if (Utils.IsHeadless())
        {
            base.OnRoomServerPlayersReady();
        }
        else
        {
            showStartButton = true;
        }
    }

    public override void OnGUI()
    {
        base.OnGUI();

        //Если сцена комнаты, рисуем интерфейс для выбора цвета рядом с кнопками
        if (Utils.IsSceneActive(RoomScene) && showRoomGUI)
        {
            GUI.backgroundColor = selectedColor;
            if (GUI.Button(new Rect(150, 300, 120, 20), "Цвет пушки"))
            {
                showColorDropdown = !showColorDropdown;
            }

            if (showColorDropdown)
            {
                for (int i = 0; i < colorOptions.Length; i++)
                {
                    GUI.backgroundColor = colorOptions[i];

                    if (GUI.Button(new Rect(150, 320 + i * 20, 120, 20), ""))
                    {
                        selectedColor = colorOptions[i];
                        selectedColorIndex = i;
                        showColorDropdown = false;

                    }
                }
            }

            GUI.backgroundColor = Color.white;
            GUI.enabled = true;
        }



        if (allPlayersReady && showStartButton && GUI.Button(new Rect(280, 300, 120, 20), "START GAME"))
        {
            
            showStartButton = false;

            ServerChangeScene(GameplayScene);
        }
    }



}
