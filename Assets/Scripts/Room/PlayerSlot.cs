using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Mirror
{
    public class PlayerSlot : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Button readyButton;
        [SerializeField] private Button notReadyButton;
        [SerializeField] private TextMeshProUGUI readyStatus;
        [SerializeField] private GameObject readyStatusGreenCase;
        [SerializeField] private GameObject readyStatusRedCase;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetNameText(string text)
        {
            nameText.text = "Player " + text;
        }
        public void SetReadyStatus(bool status)
        {
            if (status)
            {
                readyStatus.text = "Pas prêt";
                readyStatusGreenCase.SetActive(false);
                readyStatusRedCase.SetActive(true);

            }
            else
            {
                readyStatus.text = "Prêt";
                readyStatusGreenCase.SetActive(true);
                readyStatusRedCase.SetActive(false);
            }
        }
        public void SetButtonsActive(bool status)
        {
            if (status == false)
            {
                readyButton.transform.gameObject.SetActive(status);
                notReadyButton.transform.gameObject.SetActive(status);
                return;
            }
            //Debug.Log("past");
            NetworkRoomManager room = NetworkManager.singleton as NetworkRoomManager;
            if (room)
            {
                if (!Utils.IsSceneActive(room.RoomScene))
                    return;
                foreach (NetworkRoomPlayer p in room.roomSlots)
                {
                    if (p.isLocalPlayer == true)
                    {
                        //Debug.Log(p.readyToBegin);
                        if (p.readyToBegin == false)
                        {
                            readyButton.gameObject.SetActive(true);
                            notReadyButton.gameObject.SetActive(false);
                        }
                        else if (p.readyToBegin == true)
                        {
                            readyButton.gameObject.SetActive(false);
                            notReadyButton.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
        public void SetReadyStatusActive(bool status)
        {
            readyStatus.transform.gameObject.SetActive(status);
        }

        public void ChangeReadyStatus()
        {
            NetworkRoomManager room = NetworkManager.singleton as NetworkRoomManager;
            if (room)
            {
                if (!Utils.IsSceneActive(room.RoomScene))
                    return;
                
                foreach (NetworkRoomPlayer p in room.roomSlots)
                {
                    if (p.isLocalPlayer == true)
                    {
                        if (p.readyToBegin == false)
                        {
                            p.CmdChangeReadyState(true);
                        }
                        else if (p.readyToBegin == true)
                        {
                            p.CmdChangeReadyState(false);
                        }
                    }
                }
            }
            Debug.Log("End");
        }
    }
}
