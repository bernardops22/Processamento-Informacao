using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] 
    Text nameText;
    [SerializeField] 
    HPBar hpBar;

    private Pikamon pikamon;

    public void SetData(Pikamon pikamon)
    {
        this.pikamon = pikamon;
        nameText.text = pikamon.Base.Name;
        hpBar.SetHP((float)pikamon.HP/pikamon.Base.MaxHp);
    }

    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHPSmooth((float)pikamon.HP/pikamon.Base.MaxHp);
    }
}
