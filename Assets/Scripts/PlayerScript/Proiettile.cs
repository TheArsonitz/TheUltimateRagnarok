using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proiettile : MonoBehaviour
{

    private GameObject proprietario;
    private Rigidbody2D rb;

    [Header("Stats colpo")]
    public float velocita = 0.0f;
    public float danno = 0.0f;

    [Header("Settings Collisioni")]
    public LayerMask layerMuri;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Lancia(float direzione, GameObject chiHaSparato) {

        proprietario = chiHaSparato;

        if(rb == null) rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(velocita * direzione, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.gameObject == proprietario) return;

        HealthSystem vita = collision.GetComponent<HealthSystem>();
        if(vita != null) {
            vita.PrendiDanno(danno);
            DistruggiProiettile();
            return;
        }

        if ((layerMuri.value & (1 << collision.gameObject.layer)) > 0) {
            DistruggiProiettile();
        }

    }
    
    void DistruggiProiettile() {
        Destroy(gameObject);
    }

}
