using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttack : MonoBehaviour
{
    [Header("Stats Attacco")]
    public float damage = 0.0f;
    public KeyCode attacco = KeyCode.F;
    public float tempoRicaricaAttacco = 0.0f;
    protected float prossimoAttacco = 0.0f;

    [Header("Animazioni")]
    public Animator animator;

    protected virtual void Update() {
        if(Input.GetKeyDown(attacco) && Time.time >= prossimoAttacco) {

            EseguiAttacco();
            prossimoAttacco = Time.time + tempoRicaricaAttacco;
        
        }
    }

    protected abstract void EseguiAttacco();

}
