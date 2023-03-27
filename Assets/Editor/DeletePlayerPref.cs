using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DeletePlayerPref
{

    [MenuItem("PlayerPref/Clear")]
    public static void clearPlayerPref()
    {
        PlayerPrefs.DeleteAll();
    }
}
