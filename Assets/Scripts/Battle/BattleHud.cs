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

    private Pikemon _pikemon;

    public void SetData(Pikemon pikemon)
    {
        _pikemon = pikemon;
        nameText.text = pikemon.Base.Name;
        hpBar.SetHP((float)pikemon.HP/pikemon.Base.MaxHp);
    }

    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHPSmooth((float)_pikemon.HP/_pikemon.Base.MaxHp);
    }
}
