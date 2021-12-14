using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pikamon
{

    [SerializeField] PikamonBase _base;

    public Pikamon(PikamonBase pBase)
    {
        _base = pBase;
        Init();
    }

    public PikamonBase Base
    {
        get
        {
            return _base;
        }
    }
    
    public int HP {set; get;}
    
    public double Tenacity { set; get; }
    
    public double Rarity { set; get; }
    
    public List<Move> Moves { get; set; }

    public void Init(){
        
        //Base = pBase;
        HP = Base.MaxHp;

        //Generate Moves
        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            Moves.Add(new Move(move.Base));

            if (Moves.Count >= 4)
                break; 
        }
    }
    
    //COLOCAR AQUI O GERADOR DO ATAQUE
    public bool TakeDamage(Move move, Pikamon attacker)
    {
        int damage = Mathf.FloorToInt(move.Base.Power + (float) Generators.DamageFromAttacks());

        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            return true;
        }

        return false;
    }
    
    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r]; 
    }

    public void RestoreHP()
    {
        HP = Base.MaxHp;
    }
    
}
