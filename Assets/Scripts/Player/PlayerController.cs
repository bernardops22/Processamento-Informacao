using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour{

    public float moveSpeed;
    public LayerMask solidObjectsLayer;
    public LayerMask grassLayer;

    private bool isSprinting;
    [SerializeField] float sprintingSpeedMultiplier;
    public event Action OnEncountered;

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
        
        CheckForEncounters();
    }

    private bool IsWalkable(Vector3 targetPos){
        if(Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) != null){
            return false;
        }
        return true;
    }
    
    //Probabilidade de aparecer um pikamon (difere se estiver a correr ou a andar)
    private void CheckForEncounters(){
        if (Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
        {
            if(UnityEngine.Random.Range(1,101) <= 10)
            {
                animator.SetBool("isMoving",false);
                OnEncountered();
            }
        }
    }
}
