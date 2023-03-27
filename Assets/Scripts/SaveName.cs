using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveName : MonoBehaviour
{
    public GameObject namePanel;
    public GameObject panel;

    public void SetName(string s) {
        PlayerPrefs.SetString("PlayerName", s);
        Debug.Log(s);
    }

    public void changePanel() {
        namePanel.SetActive(false);
        panel.SetActive(true);
    }
}
