using UnityEngine;

public class ScreenSetup : MonoBehaviour
{
    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
}
