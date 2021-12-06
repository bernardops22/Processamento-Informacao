using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : MonoBehaviour
{
    [SerializeField] private Text messageText;
    
    private PartyMemberUI[] memberSlots;
    private List<Pikamon> pikamons;

    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUI>(true);
    }

    public void SetPartyData(List<Pikamon> pikamons)
    {
        this.pikamons = pikamons;
        for (int i = 0; i < memberSlots.Length; i++)
        {
            if (i < pikamons.Count)
            {
                memberSlots[i].gameObject.SetActive(true);
                memberSlots[i].SetData(pikamons[i]);
            }
            else
                memberSlots[i].gameObject.SetActive(false);
        }

        messageText.text = "Choose a Pikamon";
    }

    public void UpdateMemberSelection(int selectedMember)
    {
        for (int i = 0; i < pikamons.Count; i++)
        {
            if ( i == selectedMember)
                memberSlots[i].SetSelected(true);
            else memberSlots[i].SetSelected(false);
        }
    }

    public void SetMessageText(string message)
    {
        messageText.text = message;
    }
}
