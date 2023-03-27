using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.Examples.NetworkRoom
{
    [AddComponentMenu("")]
    public class MyNetworkRoomManager : NetworkRoomManager
    {
        public static new MyNetworkRoomManager singleton { get; private set; }
        public bool showButton = false;
        /// <summary>
        /// Runs on both Server and Client
        /// Networking is NOT initialized when this fires
        /// </summary>
        public override void Awake()
        {
            base.Awake();
            singleton = this;
        }

        /// <summary>
        /// This is called on the server when a networked scene finishes loading.
        /// </summary>
        /// <param name="sceneName">Name of the new scene.</param>
        public override void OnRoomServerSceneChanged(string sceneName)
        {
            // spawn the initial batch of Rewards
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

        public bool showStartButton;

        public override void OnRoomServerPlayersReady()
        {
            // calling the base method calls ServerChangeScene as soon as all players are in Ready state.
#if UNITY_SERVER
            base.OnRoomServerPlayersReady();
#else
            showStartButton = true;
            GameObject.Find("Canvas").GetComponent<RoomCanvasManager>().SetButtonStart(true);
#endif
        }

        public override void ServerChangeScene(string newSceneName)
        {
            if (newSceneName == RoomScene)
            {
                foreach (NetworkRoomPlayer roomPlayer in roomSlots)
                {
                    if (roomPlayer == null)
                        continue;

                    // find the game-player object for this connection, and destroy it
                    NetworkIdentity identity = roomPlayer.GetComponent<NetworkIdentity>();

                    if (NetworkServer.active)
                    {
                        // re-add the room object
                        roomPlayer.GetComponent<NetworkRoomPlayer>().readyToBegin = false;
                        //roomPlayer.GetComponent<MyNetworkRoomPlayer>().playerName = test;
                        NetworkServer.ReplacePlayerForConnection(identity.connectionToClient, roomPlayer.gameObject);
                    }
                }

                allPlayersReady = false;
            }

            base.ServerChangeScene(newSceneName);
        }

        public override void OnGUI()
        {
            base.OnGUI();

            if (allPlayersReady && showStartButton && GUI.Button(new Rect(150, 300, 120, 20), "START GAME"))
            {
                // set to false to hide it in the game scene
                showStartButton = false;

                ServerChangeScene(GameplayScene);
            }
        }
    }
}
