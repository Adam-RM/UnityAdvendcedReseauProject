using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mirror
{
    public class ButtonBack : NetworkBehaviour
    {  
        // Start is called before the first frame update
        public void BackToMenu()
        {
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
    }
}

