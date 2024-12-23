using TMPro;
using UnityEngine;

public class ResourcesUI : MonoBehaviour
{
    public TMP_Text medalsText;
    public TMP_Text energyText;
    public TMP_Text coinsText;

    
    public void UpdateUI(int medals, int energy, int coins)
    {
        medalsText.text = medals.ToString();
        energyText.text = energy.ToString();
        coinsText.text = coins.ToString();
    }
}
