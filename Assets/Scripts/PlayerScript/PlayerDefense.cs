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
        AggiornaComandiDaPrefs();
    }

    public void AggiornaComandiDaPrefs() {
        int setComandi = PlayerPrefs.GetInt("SetComandiP1", 0);
        if (setComandi == 1) {
            tastoParata = KeyCode.Keypad5;
        } else {
            tastoParata = KeyCode.S;
        }
    }
    
    void Update() {
        int setComandi = PlayerPrefs.GetInt("SetComandiP1", 0);
        bool parando = (setComandi == 1)
            ? (Input.GetKey(KeyCode.Keypad5) || Input.GetKey(tastoParata))
            : (Input.GetKey(KeyCode.S) || Input.GetKey(tastoParata));

        if (parando) 
            isInDifesa = true;
        else 
            isInDifesa = false;

        if(animator != null) 
            animator.SetBool("IsParando", isInDifesa);

    }
}
