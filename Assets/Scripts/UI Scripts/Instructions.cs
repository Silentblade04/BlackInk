using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class Instructions : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI instructions;
    [SerializeField] private RawImage background;

    public void showText()
    {
        if (instructions.enabled == true)
        {
            instructions.enabled = false;
        }
        else if (instructions.enabled == false)
        {
            instructions.enabled = true;
        }

        if (background.enabled == true)
        {
            background.enabled = false;
        }
        else if (background.enabled == false)
        {
            background.enabled = true;
        }
    }
}
