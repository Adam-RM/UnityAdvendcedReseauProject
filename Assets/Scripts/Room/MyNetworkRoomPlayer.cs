using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    [AddComponentMenu("")]


    public class MyNetworkRoomPlayer : NetworkRoomPlayer
    {
        [SyncVar] public string playerName = null;

        [ClientRpc]
        void SetClientName(MyNetworkRoomPlayer player, string name)
        {
            player.playerName = name;
            GameObject.Find("Canvas").GetComponentInChildren<PlayerData>().ResetPlayersPanel();
        }
        [Command]
        void CmdSetClientName(MyNetworkRoomPlayer player, string name)
        {
            SetClientName(player, name);
        }
        public override void OnStartClient()
        {
            Debug.Log("OnStartClient");
            if (isLocalPlayer)
            {
                Debug.Log("past OnStartClient");
                //CmdSetClientName(this, PlayerPrefs.GetString("PlayerName"));
            }
            GameObject.Find("Canvas").GetComponentInChildren<PlayerData>().ResetPlayersPanel();
        }

        public override void OnClientEnterRoom()
        {
            Debug.Log("OnClientEnterRoom");
            if (isLocalPlayer)
            {
                Debug.Log("past  OnClientEnterRoom");
                //CmdSetClientName(this, PlayerPrefs.GetString("PlayerName"));
            }
            GameObject.Find("Canvas").GetComponentInChildren<PlayerData>().ResetPlayersPanel();
        }

        public override void OnClientExitRoom()
        {
            //Debug.Log($"OnClientExitRoom {SceneManager.GetActiveScene().path}");
        }

        public override void IndexChanged(int oldIndex, int newIndex)
        {
            Debug.Log("IndexChanged");
            //Debug.Log($"IndexChanged {newIndex}");

            GameObject.Find("Canvas").GetComponentInChildren<PlayerData>().ResetPlayersPanel();
            //Debug.Log("past");
        }

        public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
        {
            PlayerData playerData = GameObject.Find("Canvas").GetComponentInChildren<PlayerData>();
            if (playerData == null)
                return;
            if (newReadyState ==false)
                GameObject.Find("Canvas").GetComponent<RoomCanvasManager>().SetButtonStart(false);
            playerData.ResetPlayersPanel();
            //Debug.Log("past");
            //Debug.Log($"ReadyStateChanged {newReadyState}");
        }

        public override void OnGUI()
        {
            base.OnGUI();
            //Debug.Log(index);
        }
    }
}
