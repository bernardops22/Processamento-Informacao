using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] private List<QuestionAnswers> qnA;
    [SerializeField] private GameObject exclamation;

    private bool isTrue;
    private bool isInteractable = true;
    public IEnumerator TriggerExclamation()
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
            StartCoroutine(QuizManager.Instance.ShowQuiz(qnA));
            exclamation.SetActive(false);
            isTrue = true;
            isInteractable = false;
        }
    }
}
