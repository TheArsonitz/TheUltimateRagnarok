using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefense : MonoBehaviour
{

    [Header("Difesa")]
    public KeyCode tastoParata = KeyCode.S;
    public bool isInDifesa = false;

    private Animator animator;

    
    void Start() {
        animator = GetComponent<Animator>();
    }

    
    void Update() {

        if (Input.GetKey(tastoParata)) 
            isInDifesa = true;
        else 
            isInDifesa = false;

        if(animator != null) 
            animator.SetBool("IsParando", isInDifesa);

    }
}
