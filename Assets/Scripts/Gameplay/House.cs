using UnityEngine;

public class House : MonoBehaviour, Interactable
{

    [SerializeField] GameObject playerObject;
    [SerializeField] Camera HouseCamera;
    [SerializeField] Camera PlayerCamera;

    public void Interact()
    {
        if (playerObject.transform.position == new Vector3(9.5f, 4.8f, 0))
        {
            playerObject.transform.position = new Vector3(31.5f, 0.8f, 0);
            HouseCamera.gameObject.SetActive(true);
            PlayerCamera.gameObject.SetActive(false);
        }
    }
    
}
