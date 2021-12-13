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
    [SerializeField] List<Pikamon> pikamonList;
    [SerializeField] private PikamonParty pikamonParty;
    private double[] vector = new double[3];
    [SerializeField] public List<Text> options;
    [SerializeField] private Color highlightedColor;

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
    private int currentOption = 0;

    public IEnumerator ShowQuiz(List<QuestionAnswers> qnAnswersList)
    {
        yield return new WaitForEndOfFrame();

        OnShowQuiz?.Invoke();
        
        state = QuizState.Busy;
        Update();
        
        qnA = qnAnswersList;
        quizBox.SetActive(true);
        TypeQuestion(qnA[currentQuestion].question);
    }

    private void Update()
    {
        if(state == QuizState.Busy)
            HandleOptionSelection();
    }

    public void HandleUpdate()
    {
        state = QuizState.Busy;
        if (Input.GetKeyDown(KeyCode.Z))
        {
            currentQuestion++;
            Update();
            if (currentQuestion < qnA.Count)
            {
                TypeQuestion(qnA[currentQuestion].question);
            }
            else
            {
                ChooseFirstPikamon();
                quizBox.SetActive(false);
                OnCloseQuiz?.Invoke();
            }
            state = QuizState.Free;
        }
    }
    
    private void TypeQuestion(string question)
    {
        isTyping = true;
        questionText.text = "";
        
        for (int i = 0; i < qnA[currentQuestion].answers.Length; i++)
            options[i].text = qnA[currentQuestion].answers[i];
        
        questionText.text = question;

        isTyping = false;
    }

    void HandleOptionSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            currentOption++;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            currentOption--;

        currentOption = Mathf.Clamp(currentOption, 0, 2);

        UpdateOptionSelection(currentOption);
        
        if (Input.GetKeyDown(KeyCode.Z) && currentQuestion < 6)
        {
            VectorOperation();
        }
    }

    private void VectorOperation()
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
    
    private void UpdateOptionSelection(int selectedOption)
    {
        for (int i = 0; i < options.Count; i++)
        {
            if (i == selectedOption)
                options[i].color = highlightedColor;
            else
            {
                options[i].color = Color.black;
            }
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