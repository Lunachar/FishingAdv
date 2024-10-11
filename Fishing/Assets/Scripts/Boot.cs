using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Boot : MonoBehaviour
{
    public UIManager uiManager;
    
    private GameManager _gameManager;

    private void Awake()
    {
        Debug.Log("Boot");
        GameObject gameObject = this.gameObject;
        _gameManager = new GameManager();
        _gameManager.Initialize();

        // UIManager = new UIManager();
        // UIManager.Initialize();

        if (uiManager != null)
        {
            uiManager.Initialize();
        }
        else
        {
            Debug.Log("UIManager isn't set.");
        }
    }
}
