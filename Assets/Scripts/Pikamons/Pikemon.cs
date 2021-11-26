using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pikemon{
    public PikemonBase Base {get; set;}
    public int HP {set; get;}
    
    public List<Move> Moves { get; set; }

    public Pikemon(PikemonBase pBase){
        Base = pBase;
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
    public bool TakeDamage(Move move, Pikemon attacker)
    {
        //Se nao conseguirmos fazer todas as nossas propostas podemos fazer um gerador para o critical
        //O critical pode ser removido caso nao queiramos usar como variavel aleatória

        /*float critical = 1f;
        if (Random.value * 100f <= 6.25f)
            critical = 2f;*/

        float modifiers = Random.Range(0.85f, 1f); // * critical;
        float a = 10 / 50f;
        float d = a * move.Base.Power;
        int damage = Mathf.FloorToInt(d * modifiers);

        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            return true;
        }

        return false;
    }
    
    //Podemos fazer aqui mais uma porque é necessário um random para escolher o move do enemy pokemon
    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r]; 
    }
    
}
