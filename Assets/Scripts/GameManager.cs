using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror {
    public class GameManager : NetworkBehaviour
    {
        [SerializeField] GameObject PlayersSlots;
        [SerializeField] CanvasManager canvasManager;
        bool playersAreDead = false;
        void Start()
        {

        }

        // Update is called once per frame
        void LateUpdate()
        {

            if (isServer)
            {
                CheckIfPlayersAreAllDead();
                if (playersAreDead == true)
                {
                    ShowLoseScreen();
                }
            }
        }

        [ClientRpc]
        void ShowLoseScreen()
        {
            canvasManager.SetLoseScreen();
        }

        void CheckIfPlayersAreAllDead() {
            if (PlayersSlots.transform.childCount == 0)
                return;
            foreach (Transform player in PlayersSlots.transform)
            {
                foreach (Transform t in player)
                {
                    if (t.tag == "Body" && t.gameObject.activeSelf == true)
                    {
                        return;
                    }
                }
            }
            playersAreDead = true;

        }
    }
}
