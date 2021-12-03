using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MapArea : MonoBehaviour
{
    [FormerlySerializedAs("wildPikamons")] [SerializeField] List<Pikamon> wildPikamons;
    
    
    //TODO Spawn based on pikamon rarity
    public Pikamon GetRandomWildPikamon()
    {
        var wildPikamon = wildPikamons[Random.Range(0,wildPikamons.Count)];
        wildPikamon.Init();
        return wildPikamon;
    }
}
