using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : PlayerAttack
{
    [Header("Parametri HitBox")]
    //Variabile che serve a definire quale oggetto del personaggio
    //effettuerà il colpo corpo a corpo (spada, pugno, etc...)
    public Transform puntoAttacco;
    //Variabile che definisce l'HitBox
    public float raggioAttacco = 0.5f;

    public LayerMask layerNemici;

    protected override void EseguiAttacco() {
        
        if (animator != null) {
            animator.SetTrigger("AttaccoMelee");
        }

        Collider2D[] nemiciColpiti = Physics2D.OverlapCircleAll(puntoAttacco.position, raggioAttacco, layerNemici);

        foreach (Collider2D nemico in nemiciColpiti) {
            HealthSystem vitaNemico = nemico.GetComponent<HealthSystem>();

            if (vitaNemico != null)
            {
                vitaNemico.PrendiDanno(damage);
            }

        }

    }

    //Questo metodo serve per vedere il contorno dell'HitBox così
    //si setta il raggioAttacco più facilmente
    private void OnDrawGizmosSelected() {
        if (puntoAttacco == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(puntoAttacco.position, raggioAttacco);
    }

}
