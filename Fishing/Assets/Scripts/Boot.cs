using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Boot : MonoBehaviour
{
    public UIManager _uiManager;
    
    private GameManager _gameManager;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Debug.Log("Boot");
       
        _gameManager = new GameManager();

        if (_uiManager != null)
        {
            _gameManager.Initialize(_uiManager);
        }
        else
        {
            Debug.Log("UIManager isn't set.");
        }
    }
}
