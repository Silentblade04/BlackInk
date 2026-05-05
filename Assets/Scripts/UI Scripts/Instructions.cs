using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

//help from chatgpt
public class Instructions : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI instructions;
    [SerializeField] private RawImage background;
    [SerializeField] private Button playButton;
    [SerializeField] private Button instructionButton;
    [SerializeField] private Button close;


    private void Start()
    {
        close.gameObject.SetActive(false);
    }
    public void showText()
    {
        // Show instructions + background
        instructions.enabled = true;
        background.enabled = true;

        // Hide play + instruction buttons
        playButton.gameObject.SetActive(false);
        instructionButton.gameObject.SetActive(false);

        // Show close button
        close.gameObject.SetActive(true);
    }
    public void hideText()
    {
        // Hide instructions + background
        instructions.enabled = false;
        background.enabled = false;

        // Show play + instruction buttons
        playButton.gameObject.SetActive(true);
        instructionButton.gameObject.SetActive(true);

        // Hide close button
        close.gameObject.SetActive(false);
    }
}
