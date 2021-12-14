using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour,Interactable
{
    [SerializeField] GameObject playerObject;
    [SerializeField] Camera HouseCamera;
    [SerializeField] Camera PlayerCamera;

    public void Interact()
    {
        playerObject.transform.position = new Vector3(9.5f, 4.8f, 0);
        HouseCamera.gameObject.SetActive(false);
        PlayerCamera.gameObject.SetActive(true);
    }

}