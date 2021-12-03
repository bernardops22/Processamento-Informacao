using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] 
    HPBar hpBar;

    [SerializeField] private Color highlightedColor;

    private Pikamon pikamon;

    public void SetData(Pikamon pikamon)
    {
        this.pikamon = pikamon;
        nameText.text = pikamon.Base.Name;
        hpBar.SetHP((float)pikamon.HP/pikamon.Base.MaxHp);
    }

    public void SetSelected(bool selected)
    {
        if (selected)
        {
            nameText.color = highlightedColor;
        }
        else
        {
            nameText.color = Color.black;
        }
    }
}
