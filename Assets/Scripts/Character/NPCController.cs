using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

public class NPCController : MonoBehaviour, Interactable
{

    [SerializeField] private Dialog dialog;
    [SerializeField] private GameObject exclamation;
    [SerializeField] PlayerController playerController;

    private bool isTrue = false;
    private bool isInteractable = true;
    public IEnumerator TriggerExclamation(PlayerController player)
    {
        if (!isTrue)
        {
            exclamation.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void Interact()
    {
        if (isInteractable)
        {
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
            //dar pokemon ao jogador (fazer apos ter sistema de apanhar pokemons)
            exclamation.SetActive(false);
            isTrue = true;
            isInteractable = false;
        }
    }
}
