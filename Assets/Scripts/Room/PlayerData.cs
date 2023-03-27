using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity;

namespace Mirror
{
    public class PlayerData : NetworkBehaviour
    {
        //[SyncVar(hook = nameof(GetPlayersId))] List<float> playerIds = new List<float>();
        [SerializeField] List<GameObject> playerSlots = new List<GameObject>();
        private int nbPlayers = 0;
        NetworkRoomManager room = NetworkManager.singleton as NetworkRoomManager;


        private void Start()
        {
            
        }
        private void Update()
        {
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                PrintPlayersId();
                GetMyPlayerId();
            }
            if(nbPlayers == 0)
            {
                if (room)
                {
                    if (!Utils.IsSceneActive(room.RoomScene))
                        return;
                    ResetPlayersPanel();
                    nbPlayers++;
                }
            }

        }

        private void PrintPlayersId()
        {
            if (room)
            {
                if (!Utils.IsSceneActive(room.RoomScene))
                    return;
                Debug.Log(room.roomSlots.Count);
            }
        }

        private void GetMyPlayerId()
        {
            if (room)
            {
                if (!Utils.IsSceneActive(room.RoomScene))
                    return;
                foreach (NetworkRoomPlayer p in room.roomSlots)
                {
                    if (p.isLocalPlayer  == true)
                    {
                        Debug.Log(p.index);
                    }
                }
            }
        }
        public void ResetPlayersPanel()
        {
            int x = 0;
            foreach (MyNetworkRoomPlayer p in room.roomSlots)
            {
                if (playerSlots == null)
                    return;
                if (room.roomSlots.Count <= x)
                    return;
                playerSlots[x].GetComponent<PlayerSlot>().SetNameText((p.index + 1).ToString());
                if (p.isLocalPlayer == false) {
                    playerSlots[x].GetComponent<PlayerSlot>().SetButtonsActive(false);
                    playerSlots[x].GetComponent<PlayerSlot>().SetReadyStatusActive(true);
                    if (p.readyToBegin == false)
                    {
                        playerSlots[x].GetComponent<PlayerSlot>().SetReadyStatus(true);
                    }
                    else if (p.readyToBegin == true)
                    {
                        playerSlots[x].GetComponent<PlayerSlot>().SetReadyStatus(false);
                    }
                }
                else if (p.isLocalPlayer == true)
                {
                    playerSlots[x].GetComponent<PlayerSlot>().SetButtonsActive(true);
                    playerSlots[x].GetComponent<PlayerSlot>().SetReadyStatusActive(false);
                }
                x++;
            }
        }
    }
}

/*        [SyncVar(hook = nameof(AssignSlot))]
        private string newSlotId;

        [Command]
        public void CmdSendMessage(string slotId)
        {
            newSlotId = slotId;
        }

        private void AssignSlot(string slotId)
        {
            foreach (GameObject playerSlot in playerSlots)
            {
                if (playerSlot.GetComponentInChildren<TextMeshPro>().text == "")
                    playerSlot.GetComponentInChildren<TextMeshPro>().text = slotId;
            }
        }*/