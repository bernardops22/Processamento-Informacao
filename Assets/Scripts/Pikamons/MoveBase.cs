using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Move",menuName="Pokemon/Create new move")]
public class MoveBase : ScriptableObject
{
    [SerializeField] string name;
    [SerializeField] private int power;
    [SerializeField] int pp; //Numero de vezes que se pode usar um ataque
                             //A ser junto com o attack do pokemon para criar dano de ataque
    
    public string Name {
        get { return name;}
    }

    public int PP{
        get { return pp;}
    }

    public int Power
    {
        get { return power; }
    }

}
