using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour{

    public float moveSpeed;

    private bool isSprinting;
    [SerializeField] float sprintingSpeedMultiplier;
    public event Action OnEncountered;
    public event Action<Collider2D> OnEnterTrainersView;

    private bool isMoving;
    private Vector2 input;

    private Animator animator;

    private void Awake(){
        animator = GetComponent<Animator>();
    }

   public void HandleUpdate(){
        if(!isMoving){
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            //remove diagonal movement
            if(input.x != 0) input.y = 0;
    
            if (input != Vector2.zero){
                animator.SetFloat("moveX",input.x);
                animator.SetFloat("moveY",input.y);
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;
                if (Input.GetKey(KeyCode.LeftShift))
                    isSprinting = true;
                if(IsWalkable(targetPos))
                    StartCoroutine(Move(targetPos));
            }
        }   
        animator.SetBool("isMoving", isMoving);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Interact();
        }
    }
   
   void Interact()
   {
       var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
       var interactPos = transform.position + facingDir;

       var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.i.InteractableLayer);
       if (collider != null)
       {
           collider.GetComponent<Interactable>()?.Interact();
       }
   }

    IEnumerator Move(Vector3 targetPos){

        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon){
            if (!isSprinting)
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            else
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * sprintingSpeedMultiplier * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
        isSprinting = false;
        
        OnMoveOver();
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if(Physics2D.OverlapCircle(targetPos, 0.2f, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer) != null){
            return false;
        }
        return true;
    }

    private void OnMoveOver()
    {
        CheckForEncounters();
        CheckIfInTrainersView();
    }
    
    //Probabilidade de aparecer um pikamon (difere se estiver a correr ou a andar)
    private void CheckForEncounters(){
        if (Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.GrassLayer) != null && GetComponent<PikamonParty>().GetHealthyPikamon() != null)
        {
            int probability = 25;
            if (isSprinting) probability = 75;
            
            if(UnityEngine.Random.Range(1,101) <= probability)
            {
                animator.SetBool("isMoving",false);
                OnEncountered();
            }
        }
    }

    private void CheckIfInTrainersView()
    {
        var collider = Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.FovLayer);
        if (collider != null)
        {
            OnEnterTrainersView?.Invoke(collider);
        }
    }
}
