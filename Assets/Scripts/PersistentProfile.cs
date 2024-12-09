using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentProfile : MonoBehaviour
{
    private const string KEY_NAME = "MainPlayer";
    public static PersistentProfile Instance = new PersistentProfile();

    public string UserId = "mock0000";
    public string UserName = "PatientZero";

    // TODO: In Progress Model
    public int XP = 0;
    public int Coin = 0;

    public static void Save()
    {
        if (Instance == null)
        {
            return;
        }
        PlayerPrefs.SetString(KEY_NAME, JsonUtility.ToJson(Instance));
        PlayerPrefs.Save();
    }

    public static void RemoveData()
    {
        PlayerPrefs.DeleteAll();
        Instance = new PersistentProfile();
        PlayerPrefs.Save();
    }

    public static PersistentProfile Load()
    {
        if (PlayerPrefs.HasKey(KEY_NAME))
        {
            try
            {
                var json = PlayerPrefs.GetString(KEY_NAME);
                PersistentProfile player = JsonUtility.FromJson<PersistentProfile>(json);
                Instance = player;
                return player;
            }
            catch (Exception e)
            {
                Debug.LogError($"failed to read Persistent Profile, {e}");
                return null;
            }
        }
        else
        {
            return null;
        }
    }
}
