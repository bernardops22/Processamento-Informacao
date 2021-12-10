using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerPanel : MonoBehaviour
{
    
    [SerializeField] public Text[] options;
    [SerializeField] private Color highlightedColor;
    
    public void UpdateOptionSelection(int currentOption)
    {
        for (int i = 0; i < options.Length; i++)
        {
            if (i == currentOption)
                options[i].color = highlightedColor;
            else
                options[i].color = Color.black;
        }
    }
    
}
