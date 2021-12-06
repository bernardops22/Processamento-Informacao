using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;
using Random = System.Random;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy, PartyScreen }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] private PartyScreen partyScreen;
    [SerializeField] private GameObject pikaballSprite;

    public event Action<bool> OnBattleOver;

    BattleState state;
    private int currentAction;
    private int currentMove;
    private int currentMember;

    private PikamonParty playerParty;
    private Pikamon wildPikamon;

    private PlayerController player;

    public void StartBattle(PikamonParty playerParty, Pikamon wildPikamon)
    {
        this.playerParty = playerParty;
        this.wildPikamon = wildPikamon;

        player = playerParty.GetComponent<PlayerController>();
        
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        playerUnit.Setup(playerParty.GetHealthyPikamon());
        enemyUnit.Setup(wildPikamon);
        playerHud.SetData(playerUnit.Pikamon);
        enemyHud.SetData(enemyUnit.Pikamon);

        partyScreen.Init();

        dialogBox.SetMoveNames(playerUnit.Pikamon.Moves);

        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pikamon.Base.Name} appeared.");
        yield return new WaitForSeconds(1f);

        PlayerAction();
    }

    public void PlayerAction()
    {
        state = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        dialogBox.EnableActionSelector(true);
    }

    void OpenPartyScreen()
    {
        state = BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty.Pikamons);
        partyScreen.gameObject.SetActive(true);
    }

    void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;
        
        var move = playerUnit.Pikamon.Moves[currentMove];
        move.PP--;
        yield return dialogBox.TypeDialog($"{playerUnit.Pikamon.Base.Name} used {move.Base.Name}");

        playerUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);
        enemyUnit.PlayHitAnimation();

        bool isFainted = enemyUnit.Pikamon.TakeDamage(move, playerUnit.Pikamon);
        yield return enemyHud.UpdateHP();
        
        if (isFainted)
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.Pikamon.Base.Name} Fainted");
            enemyUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(2f);
            OnBattleOver(true);
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }
    
    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;
        var move = enemyUnit.Pikamon.GetRandomMove();
        move.PP--;
        
        yield return dialogBox.TypeDialog($"{enemyUnit.Pikamon.Base.Name} used {move.Base.Name}");

        enemyUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);
        playerUnit.PlayHitAnimation();

        bool isFainted = playerUnit.Pikamon.TakeDamage(move, playerUnit.Pikamon);
        yield return playerHud.UpdateHP();
        
        if (isFainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Pikamon.Base.Name} Fainted");
            playerUnit.PlayFaintAnimation();
            
            yield return new WaitForSeconds(2f);

            var nextPikamon = playerParty.GetHealthyPikamon();
            if (nextPikamon != null)
            {
                OpenPartyScreen();
            }
            else
            {
                OnBattleOver(false);
            }
        }
        else
        {
            PlayerAction();
        }
        
    }

    public void HandleUpdate()
    {
        if (state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
        else if (state == BattleState.PartyScreen)
        {
            HandlePartyScreenSelection();
        }
    }

    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentAction;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentAction;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentAction += 2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentAction -= 2;

        currentAction = Mathf.Clamp(currentAction, 0, 3);
        
        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                //Fight
                PlayerMove();
            }else if (currentAction == 1)
            {
                //Bag
                StartCoroutine(ThrowPikaball());
            }else if (currentAction == 2)
            {
                //Pikamon
                OpenPartyScreen();
            }else if (currentAction == 3)
            {
                //Run
                OnBattleOver(true);
            }
            
        }
    }

    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentMove;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentMove;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentMove += 2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentMove -= 2;

        currentMove = Mathf.Clamp(currentMove, 0, playerUnit.Pikamon.Moves.Count - 1);

        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pikamon.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            PlayerAction();
        }
    }

    void HandlePartyScreenSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentMember;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentMember;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentMember += 2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentMember -= 2;
        
        currentMember = Mathf.Clamp(currentMember, 0, playerParty.Pikamons.Count - 1);
        
        partyScreen.UpdateMemberSelection(currentMember);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            var selectedMember = playerParty.Pikamons[currentMember];
            if (selectedMember.HP <= 0)
            {
                partyScreen.SetMessageText("You can't send out a fainted pikamon");
                return;
            }
            if (selectedMember == playerUnit.Pikamon)
            {
                partyScreen.SetMessageText("You can't switch with the same pikamon");
                return;
            }
            partyScreen.gameObject.SetActive(false);
            state = BattleState.Busy;
            StartCoroutine(SwitchPikamon(selectedMember));
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            partyScreen.gameObject.SetActive(false);
            PlayerAction();
        }
    }

    IEnumerator SwitchPikamon(Pikamon newPikamon)
    {
        if (playerUnit.Pikamon.HP > 0)
        {
            yield return dialogBox.TypeDialog($"Come back {playerUnit.Pikamon.Base.Name}");
            playerUnit.PlayFaintAnimation();
            yield return new WaitForSeconds(2f);
        }

        playerUnit.Setup(newPikamon);
        playerHud.SetData(newPikamon);
        dialogBox.SetMoveNames(newPikamon.Moves);
        yield return dialogBox.TypeDialog($"Go {newPikamon.Base.Name}!");
        yield return new WaitForSeconds(1f);

        StartCoroutine(EnemyMove());
    }

    IEnumerator ThrowPikaball()
    {
        dialogBox.EnableActionSelector(false);
        state = BattleState.Busy;
        
        yield return dialogBox.TypeDialog($"{player.name} used PIKABALL!");
        
        var pikaballObj = Instantiate(pikaballSprite, playerUnit.transform.position - new Vector3(2,0), Quaternion.identity);
        var pikaball = pikaballObj.GetComponent<SpriteRenderer>();
        
        //Animations
        yield return pikaball.transform.DOJump(enemyUnit.transform.position + new Vector3(0,2), 2f, 1, 1f).WaitForCompletion();
        yield return enemyUnit.PlayCaptureAnimation();
        yield return pikaball.transform.DOMoveY(enemyUnit.transform.position.y - 0.9f, 0.5f).WaitForCompletion();

        int shakeCount = TryToCatchPikamon(enemyUnit.Pikamon);
        
        for (int i = 0; i < Mathf.Min(shakeCount,3); ++i)
        {
            yield return new WaitForSeconds(0.5f);
            yield return pikaball.transform.DOPunchRotation(new Vector3(0, 0, 10f), 0.8f).WaitForCompletion();
        }

        if (shakeCount == 4)
        {
            //Pikamon is caught
            yield return dialogBox.TypeDialog($"{enemyUnit.Pikamon.Base.Name} was caught");
            yield return pikaball.DOFade(0,1.5f).WaitForCompletion();
            
            playerParty.AddPikamon(enemyUnit.Pikamon);
            yield return dialogBox.TypeDialog($"{enemyUnit.Pikamon.Base.Name} has been added to your party");
            yield return new WaitForSeconds(1f);
            
            Destroy(pikaball);
            OnBattleOver(true);
        }
        else
        {
            //Pikamon broke out
            yield return new WaitForSeconds(1f);
            yield return pikaball.DOFade(0,0.2f);
            yield return enemyUnit.PlayBreakOutAnimation();
            
            yield return dialogBox.TypeDialog($"{enemyUnit.Pikamon.Base.Name} broke free");
            Destroy(pikaball);
            StartCoroutine(EnemyMove());
        }
    }

    //Probabilidade de apanhar um pikamon
    int TryToCatchPikamon(Pikamon pikamon)
    {
        double curHpPercentage = (double) pikamon.HP / pikamon.Base.MaxHp;
        double catchRate = Generators.CatchRate() * curHpPercentage;
        if (catchRate < 0.5f)
            return 4;
        return new Random().Next(0, 4);
    }
}