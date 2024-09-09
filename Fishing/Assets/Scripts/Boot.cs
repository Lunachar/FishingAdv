using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boot : MonoBehaviour
{
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = new GameManager();
        _gameManager.Initialize();
    }
}
