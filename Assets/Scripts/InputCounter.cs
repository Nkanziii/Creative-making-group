using UnityEngine;
using TMPro;

public class InputCounter : MonoBehaviour
{
    public TextMeshProUGUI counterText;

    private int leftCount = 0;
    private int rightCount = 0;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            leftCount++;
            UpdateText();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            rightCount++;
            UpdateText();
        }


    }

    void UpdateText()
    {
        int total = leftCount + rightCount;
counterText.text = "Sender1: " + total;
    }
}
