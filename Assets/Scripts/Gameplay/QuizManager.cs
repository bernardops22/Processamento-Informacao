using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public enum QuizState {Free, Busy}

public class QuizManager : MonoBehaviour
{
    private List<QuestionAnswers> qnA;
    [SerializeField] private GameObject quizBox;
    [SerializeField] private Text questionText;
    [SerializeField] private int lettersPerSecond;
    [SerializeField] private AnswerPanel answerPanel;
    [SerializeField] List<Pikamon> pikamonList;
    [SerializeField] private PikamonParty pikamonParty;
    private double[] vector = new double[3];

    public event Action OnShowQuiz;
    public event Action OnCloseQuiz;

    private QuizState state;
    public static QuizManager Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
    }
    
    private bool isTyping;
    private int currentQuestion = 0;

    public IEnumerator ShowQuiz(List<QuestionAnswers> qnAnswersList)
    {
        yield return new WaitForEndOfFrame();
        
        OnShowQuiz?.Invoke();
        
        state = QuizState.Busy;
        
        qnA = qnAnswersList;
        quizBox.SetActive(true);
        StartCoroutine(TypeQuestion(qnA[currentQuestion].question));
        Update();
        currentQuestion++;
    }

    private void Update()
    {
        if(state == QuizState.Busy)
            HandleOptionSelection();
    }

    public void HandleUpdate()
    {
        state = QuizState.Busy;
        if (Input.GetKeyDown(KeyCode.Z) && !isTyping)
        {
            if (currentQuestion < qnA.Count)
            {
                StartCoroutine(TypeQuestion(qnA[currentQuestion].question));
                Update();
                currentQuestion++;
            }
            else
            {
                ChooseFirstPikamon();
                quizBox.SetActive(false);
                OnCloseQuiz?.Invoke();
            }
        }
    }
    
    private IEnumerator TypeQuestion(string question)
    {
        isTyping = true;
        questionText.text = "";
        
        for (int i = 0; i < qnA[currentQuestion].answers.Length; i++)
            answerPanel.options[i].text = qnA[currentQuestion].answers[i];

        foreach (var letter in question.ToCharArray()){
            questionText.text += letter;
            yield return new WaitForSeconds(1f/lettersPerSecond);
        }

        isTyping = false;
    }

    private int currentOption;
    
    void HandleOptionSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentOption;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentOption;

        currentOption = Mathf.Clamp(currentOption, 0, 2);

        answerPanel.UpdateOptionSelection(currentOption);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentOption == 0)
            {
                vector[0] += 0.2;
            }
            else if (currentOption == 1)
            {
                vector[1] += 0.2;
            }
            else if (currentOption == 2)
            {
                vector[2] += 0.2;
            }
            state = QuizState.Free;
        }
    }

    public void ChooseFirstPikamon()
    {
        var firstPikamon = pikamonList[0];
        
        int pikamon = Generators.firstPikamon(vector);
        
        if (pikamon == 1)
            firstPikamon = pikamonList[1];
        else if (pikamon == 2)
            firstPikamon = pikamonList[2];
        
        firstPikamon.Init();
        pikamonParty.AddPikamon(new Pikamon(firstPikamon.Base));
    }
}