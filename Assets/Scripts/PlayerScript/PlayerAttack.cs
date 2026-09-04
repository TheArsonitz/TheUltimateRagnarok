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

    protected virtual void Start() {
        AggiornaComandiDaPrefs();
    }

    public void AggiornaComandiDaPrefs() {
        int setComandi = PlayerPrefs.GetInt("SetComandiP1", 0);
        if (setComandi == 1) {
            attacco = KeyCode.Keypad0;
        } else {
            attacco = KeyCode.Space;
        }
    }

    protected virtual void Update() {
        int setComandi = PlayerPrefs.GetInt("SetComandiP1", 0);
        bool attacca = (setComandi == 1)
            ? (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(attacco))
            : (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(attacco));

        if(attacca && Time.time >= prossimoAttacco) {

            EseguiAttacco();
            prossimoAttacco = Time.time + tempoRicaricaAttacco;
        
        }
    }

    protected abstract void EseguiAttacco();

}
