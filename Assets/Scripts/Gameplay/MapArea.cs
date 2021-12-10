using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    [SerializeField] List<Pikamon> wildPikamons;
    
    
    //TODO Spawn based on pikamon rarity
    public Pikamon GetRandomWildPikamon()
    {
        var wildPikamon = wildPikamons[Generators.Rarity()];
        wildPikamon.Init();
        return wildPikamon;
    }
}
