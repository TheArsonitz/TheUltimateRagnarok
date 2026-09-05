using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Stats")]
    //Variabili relative alle caratteristiche del Player
    public float speed = 0.0f; 
    public float jump = 0.0f;
    private Rigidbody2D rb;
    private float horizontalPos;
    
    //Variabili relative al salto e al doppio salto
    public LayerMask groundLayer;
    private int extraJump = 1;
    public bool versoDestra = true;

    [Header("Comandi")]
    //Variabili relative al set di comandi
    public string asseX = "Horizontal";
    public KeyCode tastoSalto = KeyCode.Space;

    //Variabile per animazioni
    [Header("Animazioni")]
    public Animator animator;

    public void AggiornaComandiDaPrefs() {
        int setComandi = PlayerPrefs.GetInt("SetComandiP1", 0);
        if (setComandi == 1) {
            tastoSalto = KeyCode.Keypad8;
        } else {
            tastoSalto = KeyCode.W;
        }
    }

    bool IsATerra() {
        Collider2D myCol = GetComponent<Collider2D>();
        if (myCol != null) myCol.enabled = false;
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Abs(transform.localScale.y) / 2f + 0.3f, groundLayer);
        
        if (myCol != null) myCol.enabled = true;
        
        return hit.collider != null;
    }

    void Jump() {
        rb.velocity = new Vector2(rb.velocity.x, jump);
    }

    void Flip() {
        versoDestra = !versoDestra;

        Vector3 copiaScala = transform.localScale;
        copiaScala.x *= -1;
        transform.localScale = copiaScala;
    }

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        AggiornaComandiDaPrefs();
    }

    // Update is called once per frame
    void Update() {

        PlayerDefense difesa = GetComponent<PlayerDefense>();

        if (difesa != null && difesa.isInDifesa)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        int setComandi = PlayerPrefs.GetInt("SetComandiP1", 0);
        if (setComandi == 1) {
            // Set 2: Tastierino (4 = Sx, 6 = Dx)
            horizontalPos = 0f;
            if (Input.GetKey(KeyCode.Keypad6)) horizontalPos += 1f;
            if (Input.GetKey(KeyCode.Keypad4)) horizontalPos -= 1f;
        } else {
            // Set 1: WASD (A = Sx, D = Dx)
            horizontalPos = 0f;
            if (Input.GetKey(KeyCode.D)) horizontalPos += 1f;
            if (Input.GetKey(KeyCode.A)) horizontalPos -= 1f;
            if (horizontalPos == 0f && !string.IsNullOrEmpty(asseX)) horizontalPos = Input.GetAxisRaw(asseX);
        }

        if (horizontalPos > 0 && !versoDestra) 
            Flip();
        else if (horizontalPos < 0 && versoDestra) 
            Flip();

        // extraJump viene resettato solo quando tocchiamo terra
        if(IsATerra()) {
            extraJump = 1;
        }

        bool vuoleSaltare = (setComandi == 1)
            ? (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(tastoSalto))
            : (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(tastoSalto));

        if (vuoleSaltare) {
            if (IsATerra()) {
                Jump();
            } else if (extraJump > 0)
            {
                Jump();
                extraJump--;
            }
        }

        if (animator != null) {
            animator.SetFloat("Speed", Mathf.Abs(horizontalPos));
        }

    }

    void FixedUpdate() {
        rb.velocity = new Vector2(horizontalPos*speed, rb.velocity.y);
    }

}
