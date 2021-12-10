using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PikamonParty : MonoBehaviour
{
    [SerializeField] List<Pikamon> pikamons;

    public List<Pikamon> Pikamons
    {
        get
        {
            return pikamons;
        }
    }
    
    private void Start()
    {
        foreach (var pikamon in pikamons)
        {
            pikamon.Init();
        }
    }

    public Pikamon GetHealthyPikamon()
    {
        return pikamons.Where(x => x.HP > 0).FirstOrDefault();
    }

    public void AddPikamon(Pikamon newPikamon)
    {
        if (pikamons.Count < 6)
        {
            pikamons.Add(newPikamon);
        }
    }
}
