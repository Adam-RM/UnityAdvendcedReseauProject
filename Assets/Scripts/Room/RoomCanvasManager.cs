using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    public class RoomCanvasManager : MonoBehaviour
    {
        [SerializeField] GameObject StartButton;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetButtonStart(bool status)
        {
            StartButton.SetActive(status);
        }

        public void StartGame()
        {
            Mirror.Examples.NetworkRoom.MyNetworkRoomManager myNetworkRoomManager = GameObject.Find("NetworkRoomManager").GetComponent<Mirror.Examples.NetworkRoom.MyNetworkRoomManager>();
            myNetworkRoomManager.ServerChangeScene(myNetworkRoomManager.GameplayScene);
        }
    }
}
