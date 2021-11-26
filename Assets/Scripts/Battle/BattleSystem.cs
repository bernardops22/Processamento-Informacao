using System;
using System.Collections;
using UnityEngine;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;

    public event Action<bool> OnBattleOver;

    BattleState state;
    private int currentAction;
    private int currentMove;

    private PikemonParty playerParty;
    private Pikemon wildPikemon;

    public void StartBattle(PikemonParty playerParty, Pikemon wildPikemon)
    {
        this.playerParty = playerParty;
        this.wildPikemon = wildPikemon;
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        playerUnit.Setup(playerParty.GetHealthyPikemon());
        enemyUnit.Setup(wildPikemon);
        playerHud.SetData(playerUnit.Pikemon);
        enemyHud.SetData(enemyUnit.Pikemon);

        dialogBox.SetMoveNames(playerUnit.Pikemon.Moves);

        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pikemon.Base.Name} appeared.");
        yield return new WaitForSeconds(1f);

        PlayerAction();
    }

    public void PlayerAction()
    {
        state = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        dialogBox.EnableActionSelector(true);
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
        
        var move = playerUnit.Pikemon.Moves[currentMove];
        move.PP--;
        yield return dialogBox.TypeDialog($"{playerUnit.Pikemon.Base.Name} used {move.Base.Name}");

        playerUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);
        enemyUnit.PlayHitAnimation();

        bool isFainted = enemyUnit.Pikemon.TakeDamage(move, playerUnit.Pikemon);
        yield return enemyHud.UpdateHP();
        
        if (isFainted)
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.Pikemon.Base.Name} Fainted");
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
        var move = enemyUnit.Pikemon.GetRandomMove();
        move.PP--;
        
        yield return dialogBox.TypeDialog($"{enemyUnit.Pikemon.Base.Name} used {move.Base.Name}");

        enemyUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);
        playerUnit.PlayHitAnimation();

        bool isFainted = playerUnit.Pikemon.TakeDamage(move, playerUnit.Pikemon);
        yield return playerHud.UpdateHP();
        
        if (isFainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Pikemon.Base.Name} Fainted");
            playerUnit.PlayFaintAnimation();
            
            yield return new WaitForSeconds(2f);

            var nextPikemon = playerParty.GetHealthyPikemon();
            if (nextPikemon != null)
            {
                playerUnit.Setup(nextPikemon);
                playerHud.SetData(nextPikemon);

                dialogBox.SetMoveNames(nextPikemon.Moves);

                yield return dialogBox.TypeDialog($"Go {nextPikemon.Base.Name}!");
                yield return new WaitForSeconds(1f);

                PlayerAction();
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
    }

    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(currentAction < 1)
                ++currentAction;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction > 0)
                --currentAction;
        }

        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                //Fight
                PlayerMove();
            }else if (currentAction == 1)
            {
                //Run
            }
        }
    }

    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(currentMove < playerUnit.Pikemon.Moves.Count -1)
                ++currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMove > 0)
                --currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMove < playerUnit.Pikemon.Moves.Count -2)
                currentMove+=2;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMove > 1)
                currentMove-=2;
        }

        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pikemon.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
        }
    }
}