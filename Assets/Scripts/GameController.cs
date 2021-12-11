using UnityEngine;

public enum GameState {FreeRoam, Battle, Quiz}

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    [SerializeField] private QuizManager quizManager;

    GameState state;

    private void Start()
    {
        playerController.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;

        playerController.OnEnterTrainersView += (Collider2D trainerCollider) =>
        {
           var trainer = trainerCollider.GetComponentInParent<NPCController>();
           if (trainer != null)
           {
               StartCoroutine(trainer.TriggerExclamation());
           }
        };

        QuizManager.Instance.OnShowQuiz += () =>
        {
            state = GameState.Quiz;
        };
        
        QuizManager.Instance.OnCloseQuiz += () =>
        {
            if (state == GameState.Quiz)
                state = GameState.FreeRoam;
        };
    }

    void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerParty = playerController.GetComponent<PikamonParty>();

        var wildPikamon = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildPikamon();
        var wildPikamonCopy = new Pikamon(wildPikamon.Base);
        
        battleSystem.StartBattle(playerParty, wildPikamonCopy);
    }

    void EndBattle(bool won)
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
        else if (state == GameState.Quiz)
        {
            quizManager.HandleUpdate();
        }
    }
}