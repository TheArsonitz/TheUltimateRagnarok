using UnityEngine;

public class MeleeAttack : PlayerAttack
{
    [Header("Parametri HitBox")]
    public Transform puntoAttacco;
    public float raggioAttacco = 0.5f;
    public LayerMask layerNemici;

    protected override void EseguiAttacco() 
    {
        if (animator != null) {
            animator.SetTrigger("AttaccoMelee");
        }

        Vector3 posAttacco = puntoAttacco != null ? puntoAttacco.position : transform.position;
        Collider2D[] nemiciColpiti = Physics2D.OverlapCircleAll(posAttacco, raggioAttacco, layerNemici);

        foreach (Collider2D nemico in nemiciColpiti) 
        {
            // Evita di auto-colpirsi!
            if (nemico.gameObject == this.gameObject) continue;

            IDamageable target = nemico.GetComponent<IDamageable>();
            if (target != null)
            {
                target.PrendiDanno(damage);
            }
        }
    }

    private void OnDrawGizmosSelected() 
    {
        Vector3 posAttacco = puntoAttacco != null ? puntoAttacco.position : transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(posAttacco, raggioAttacco);
    }
}
