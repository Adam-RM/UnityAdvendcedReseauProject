using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    public class BornFireEvent : NetworkBehaviour
    {
        [SerializeField] GameObject PlayersSlots;
        [SerializeField] List<Transform> playerPos = new List<Transform>();
        [SerializeField] ParticleSystem fire;
        [SerializeField] AudioSource fireSound;
        List<GameObject> playerToRevive = new List<GameObject>();
        private bool alreadyActive = false;
        int index = 0;
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        [TargetRpc]
        public void SetPos(NetworkConnectionToClient target, Vector3 pos, NetworkIdentity player)
        {
            player.transform.position = pos;
            Debug.Log("past");
        }

        public void RespawnPlayer(NetworkIdentity player)
        {

            Debug.Log("In respawn player");
            if (index >= playerPos.Count)
                return;
            SetPos(player.connectionToClient, playerPos[index].position, player);
            //player.transform.position = playerPos[index].position;
            index++;
        }
        [ClientRpc]
        private void RevivePlayers()
        {
            Debug.Log("Past in Revive Player");

            foreach(Transform player in PlayersSlots.transform)
            {
                GameObject myPlayer = null;
                foreach (Transform t in player)
                {
                    if (t.tag == "Body" && t.gameObject.activeSelf == false && player.GetComponent<PlayerController>().enabled == true)
                    {
                        Debug.Log("Find our player");
                        Debug.Log("Try to revive");
                        Debug.Log("Try to Respawn");
                        myPlayer = player.gameObject;
                        //player.GetComponent<PlayerNetwork>().CmdRespawnPlayer(this, player.GetComponent<NetworkIdentity>());
                    }
                }
                if (myPlayer != null)
                {

                    if (index >= playerPos.Count)
                        return;
                    player.transform.position = playerPos[index].position;
                    index++;
                    
                    myPlayer.GetComponent<PlayerNetwork>().CmdRespawnPlayer(this, myPlayer.GetComponent<NetworkIdentity>());
                    player.GetComponent<PlayerController>().Revive();
                }
            }

        }
        
        [ClientRpc] 
        void ShowFirePS()
        {
            fire.Play();
            fireSound.Play();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isServer && alreadyActive == false)
            {
                if(other.tag  == "Player")
                {
                    alreadyActive = true;
                    ShowFirePS();
                    RevivePlayers();
                }
            }
        }
    }
}