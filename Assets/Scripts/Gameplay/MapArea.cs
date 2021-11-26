using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    [SerializeField] List<Pikemon> wildPikemons;
    
    
    //TODO Spawn based on pikemon rarity
    public Pikemon GetRandomWildPikemon()
    {
        var wildPikemon = wildPikemons[Random.Range(0,wildPikemons.Count)];
        wildPikemon.Init();
        return wildPikemon;
    }
}
