using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    [Header("Componenti")]
    public Transform puntoSparo;
    public GameObject proiettilePrefab;

    [Header("Parametri")]
    public KeyCode tastoSparo = KeyCode.F;
    public float tempoRicarica = 0.0f;
    private float prossimoSparo = 0.0f;

    
    void Update() {
        
        if(Input.GetKeyDown(tastoSparo) && Time.time >= prossimoSparo) {
            Spara();
            prossimoSparo = Time.time + tempoRicarica;
        }

    }


    void Spara() {

        GameObject colpo = Instantiate(proiettilePrefab, puntoSparo.position, Quaternion.identity); 

        Proiettile scriptProiettile = colpo.GetComponent<Proiettile>();

        if(scriptProiettile != null ) {

            float direzione = Mathf.Sign(transform.localScale.x);
            scriptProiettile.Lancia(direzione, gameObject);

        }


    }

}
