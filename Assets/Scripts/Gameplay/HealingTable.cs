using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingTable : MonoBehaviour, Interactable
{
    [SerializeField] GameObject playerObject;
    private Animator animator;

    public event Action onHealingStart;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        List<Pikamon> pokemons = playerObject.GetComponent<PikamonParty>().Pikamons;
        if (pokemons.Count > 0)
        {

            onHealingStart();

            animator.SetBool("playHeal", true);

            foreach (var pokemon in pokemons)
            {
                Debug.Log(pokemon.HP);
                pokemon.RestoreHP();
                Debug.Log(pokemon.HP);
            }

            StartCoroutine(waitForEnd());
        }
    }

    IEnumerator waitForEnd()
    {       
        yield return new WaitForSeconds(1.85f); 
        animator.SetBool("playHeal", false);
        onHealingStart();
    }
}