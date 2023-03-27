using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] GameObject PlayerHUD;
    [SerializeField] GameObject ObserverHUD;
    [SerializeField] GameObject LoseScreen;
    [SerializeField] GameObject VictoryScreen;
    [SerializeField] GameObject PausedPanel;
    [SerializeField] GameObject SettingsPanel;


    [SerializeField] Slider volumeSlider;
    [SerializeField] Slider sensitivitySlider;

    List<Transform> playersview = new List<Transform>();
    int playerIndex = 0;
    bool isPlayerView = true;
    public bool isPaused = false;

    List<Transform> playerToRemove = new List<Transform>();
    List<Transform> playerToAdd = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {    
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerView == false)
        {
            RefreshPlayerList();
        }
    }

    private void RefreshPlayerList()
    {
        foreach (Transform player in playersview)
        {
            foreach (Transform t in player)
            {
                if (t.tag == "Body" && t.gameObject.activeSelf && player.GetComponent<Mirror.NetworkIdentity>().isOwned == false)
                {
                    if (playersview.Contains(player) == false)
                        playerToAdd.Add(player);
                }
                else if (t.tag == "Body" && playersview.Contains(player) == true && player.GetComponent<Mirror.NetworkIdentity>().isOwned == false)
                    playerToRemove.Add(player);
            }
        }
        foreach (Transform player in playerToAdd)
            playersview.Add(player);
        playerToAdd.Clear();
        foreach (Transform player in playerToRemove)
            playersview.Remove(player);
        playerToRemove.Clear();

    }
    public void SetObserversHUD()
    {
        ObserverHUD.SetActive(true);
        PlayerHUD.SetActive(false);
        isPlayerView = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void SetPlayerHUD()
    {
        ObserverHUD.SetActive(false);
        PlayerHUD.SetActive(true);
        isPlayerView = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playersview.Clear();
        SetMyPlayerView();

    }

    public void SetLoseScreen()
    {
        ObserverHUD.SetActive(false);
        PlayerHUD.SetActive(false);
        isPlayerView = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        LoseScreen.SetActive(true);
    }

    public void SetVictoryScreen()
    {
        ObserverHUD.SetActive(false);
        PlayerHUD.SetActive(false);
        isPlayerView = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        VictoryScreen.SetActive(true);
    }

    public void Paused()
    {
        if (isPaused  == false)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            PausedPanel.SetActive(true);
            isPaused = true;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            PausedPanel.SetActive(false);
            SettingsPanel.SetActive(false);
            isPaused = false;
        }
    }

    public void SetVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }

    public void SetSensitivity()
    {
        GameObject players = GameObject.Find("Players");
        foreach (Transform player in players.transform)
        {
            foreach (Transform t in player)
            {
                if (player.GetComponent<Mirror.PlayerController>().enabled == true)
                    player.GetComponent<Mirror.PlayerController>().sensitivity = sensitivitySlider.value;
            }
        }
    }
    void SetMyPlayerView()
    {
        GameObject players = GameObject.Find("Players");
        foreach (Transform player in players.transform)
        {
            foreach (Transform t in player)
            {
                if (t.tag == "Body" && t.gameObject.activeSelf && player.GetComponent<Mirror.PlayerController>().enabled == true)
                    GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>().Follow = t;
            }
        }
    }
    public bool SetDeathView()
    {
        if (playersview.Count == 0)
        {
            GameObject players = GameObject.Find("Players");
            foreach (Transform player in players.transform)
            {
                foreach (Transform t in player)
                {
                    if (t.tag == "Body" && t.gameObject.activeSelf && player.GetComponent<Mirror.NetworkIdentity>().isOwned == false)
                        playersview.Add(player);
                }
            }
            if (playersview.Count == 0)
                return false;
            foreach (Transform t in playersview[0])
            {
                if (t.tag == "CameraTarget")
                {
                    GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>().Follow = t;
                    return true;
                }
            }
        }
        return false;
        //Transform target = null;
        /*if (t.tag == "Body" && t.gameObject.activeSelf && player.GetComponent<NetworkIdentity>().isOwned == false)
            target = player;
    }
    if (target != null)
    {
        foreach (Transform t in player)
        {
            if (t.tag == "CameraTarget")
            {
                GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>().Follow = t;
                return true;
            }
        }
    }*/
    }

    public void NextPlayerView()
    {
        playerIndex++;
        if (playerIndex >= playersview.Count)
            playerIndex = 0;
        foreach (Transform t in playersview[playerIndex])
        {
            if (t.tag == "CameraTarget")
                GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>().Follow = t;
        }
    }

    public void PreviousPlayerView()
    {
        playerIndex--;
        if (playerIndex < 0)
            playerIndex = playersview.Count - 1;
        foreach (Transform t in playersview[playerIndex])
            if (t.tag == "CameraTarget")
                GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>().Follow = t;
    }

    public void LeaveGame()
    {
        GameObject players = GameObject.Find("Players");
        foreach (Transform player in players.transform)
        {
            if (player.GetComponent<Mirror.PlayerController>().enabled == true)
                player.GetComponent<Mirror.PlayerNetwork>().GoToMainMenu();
        }
    }
}
