﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMusicScript : MonoBehaviour
{
    private static TitleMusicScript instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}