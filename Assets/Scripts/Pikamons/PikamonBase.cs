using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new pokemon")]
public class PikamonBase : ScriptableObject{
    [SerializeField] string name;
    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;

//Base Stats
    [SerializeField] int maxHp;
    [SerializeField] int rarity;
    
    [SerializeField] List<LearnableMove> learnableMoves;

    public string Name{
        get{return name;}
    }

    public Sprite FrontSprite{
        get{return frontSprite;}
    }

    public Sprite BackSprite{
        get{return backSprite;}
    }

    public int MaxHp{
        get{return maxHp;}
    }

    public int Rarity
    {
        get { return rarity; }
    }

    public List<LearnableMove> LearnableMoves {
        get{return learnableMoves;}
    }
}

[System.Serializable]

public class LearnableMove{
    [SerializeField] MoveBase moveBase;

    public MoveBase Base{
        get{return moveBase;}
    }
}
