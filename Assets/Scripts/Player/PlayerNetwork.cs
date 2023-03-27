using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mirror
{
    public class PlayerNetwork : NetworkBehaviour
    {
        //public string myName = "";
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }

        [ClientRpc]
        private void HideHealthBar(bool status, NetworkIdentity target)
        {
            if (status)
            {
                foreach (Transform t in target.transform)
                {
                    if (t.tag == "PlayerUI")
                    {
                        t.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                foreach (Transform t in target.transform)
                {
                    if (t.tag == "PlayerUI" && target.isOwned  == false)
                        t.gameObject.SetActive(true);
                }
            }
        }

        [Command]
        public void CmdHideHealthBar(bool status, NetworkIdentity target)
        {
            HideHealthBar(status, target);
        }

        [ClientRpc]
        private void HideBody(bool status, NetworkIdentity target)
        {
            if (status)
            {
                foreach (Transform t in target.transform)
                {
                    if (t.tag == "Body")
                    {
                        t.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                foreach (Transform t in target.transform)
                {
                    if (t.tag == "Body")
                        t.gameObject.SetActive(true);
                }
            }
        }

        [Command]
        public void CmdHideBody(bool status, NetworkIdentity target)
        {
            HideBody(status, target);
        }


        [ClientRpc]
        void SetPlayerSlot()
        {
            foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
            {
                p.transform.parent = GameObject.Find("Players").transform;
            }
            /*
            Debug.Log(player.transform.parent);
            player.transform.parent = GameObject.Find("Players").transform;
            Debug.Log(player.transform.parent);
            Debug.Log("End SetPlayerSlot");*/
        }

        [Command]
        public void CmdSetPlayerSlot()
        {

            SetPlayerSlot();
        }

        [Command]
        public void CmdRespawnPlayer(BornFireEvent fire, NetworkIdentity player)
        {
            fire.RespawnPlayer(player);
        }

        public void GoToMainMenu()
        {
            Debug.Log("past");
            if (isServer)
            {
                NetworkRoomManager.singleton.StopHost();
                NetworkRoomManager.singleton.StopClient();

                SceneManager.LoadScene("OfflineScene", LoadSceneMode.Single);
            }
            else
            {
                NetworkRoomManager.singleton.StopClient();
                SceneManager.LoadScene("OfflineScene", LoadSceneMode.Single);
            }

        }

        [ClientRpc]
        void SetName(string name)
        {
            //myName = name;
        }
        [Command]
        public void CmdSetName(string name)
        {
            SetName(name);
        }
    }
}