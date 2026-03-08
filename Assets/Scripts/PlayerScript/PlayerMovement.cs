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

    bool IsATerra() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.7f, groundLayer);
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
    }

    // Update is called once per frame
    void Update() {

        PlayerDefense difesa = GetComponent<PlayerDefense>();

        if (difesa != null && difesa.isInDifesa)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        horizontalPos = Input.GetAxisRaw(asseX);

        if (horizontalPos > 0 && !versoDestra) 
            Flip();
        else if (horizontalPos < 0 && versoDestra) 
            Flip();


        if(IsATerra() ) {
            extraJump = 1;
        }

        if (Input.GetKeyDown(tastoSalto)) {
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
