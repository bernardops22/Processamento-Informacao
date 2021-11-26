using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PikemonParty : MonoBehaviour
{
    [SerializeField] List<Pikemon> pikemons;

    private void Start()
    {
        foreach (var pikemon in pikemons)
        {
            pikemon.Init();
        }
    }

    public Pikemon GetHealthyPikemon()
    {
        return pikemons.Where(x => x.HP > 0).FirstOrDefault();
    }
}
