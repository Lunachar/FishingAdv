using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FishingSystem
{
    private float _castDistance;
    private float _depth;
    private string _bait;

    public void Initialize()
    {
        _castDistance = 0;
        _depth = 0;
        _bait = "Default Bait";
    }

    public void SetCastDistance(float distance)
    {
        _castDistance = distance;
    }

    public void SetDepth(float depth)
    {
        _depth = depth;
    }

    public void SetBait(string bait)
    {
        _bait = bait;
    }

    public void StartFishing()
    {
        throw new NotImplementedException("start fishing logic");
    }

    public string GetCurrentBait()
    {
        throw new NotImplementedException();
    }
}
