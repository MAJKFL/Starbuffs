using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour
{
    public static int HighScore
    {
        get
        {
            if (PlayerPrefs.GetInt("hs") != null) return PlayerPrefs.GetInt("hs");
            else return 0;
        }
        set
        {
            PlayerPrefs.SetInt("hs", value);
        }
    }
}
