using UnityEngine;
using TMPro;

public class InputCounterSender2 : MonoBehaviour
{
    public TextMeshProUGUI counterText;

    private int eCount = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            eCount++;
            UpdateText();
        }
    }

    void UpdateText()
    {
        counterText.text = "Sender2: " + eCount;
    }
}
