using UnityEngine;

public class NemicoAI : MonoBehaviour
{
    public enum TipoBoss { CorpoACorpo, Distanza }
    public enum StatoBoss { Avvicinamento, Fuga, Attacco, Recupero }

    [Header("Impostazioni Boss")]
    public TipoBoss stileDiCombattimento = TipoBoss.CorpoACorpo;
    public StatoBoss statoAttuale = StatoBoss.Avvicinamento;

    [Header("Statistiche Combattimento")]
    public float velocita = 3f;
    public float raggioAttacco = 1.5f; 
    public float tempoTraAttacchi = 2f;
    
    [Header("Solo per Combattimento a Distanza")]
    public float raggioFuga = 4f; 

    [Header("Armi a Distanza (Ignora se Corpo a Corpo)")]
    public GameObject proiettilePrefab;
    public Transform puntoSparo;

    [Header("Componenti opzionali")]
    public Animator animator;

    private Transform giocatore;
    private float timer;

    void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null)
        {
            giocatore = obj.transform;
        }
        if (animator == null) animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (giocatore == null || Time.timeScale == 0f) return;

        GiraVersoAvversario();
        float distanza = Vector2.Distance(transform.position, giocatore.position);

        switch (statoAttuale)
        {
            case StatoBoss.Avvicinamento:
                if (stileDiCombattimento == TipoBoss.Distanza && distanza < raggioFuga)
                {
                    statoAttuale = StatoBoss.Fuga;
                }
                else if (distanza > raggioAttacco)
                {
                    MuovitiVersoGiocatore();
                }
                else
                {
                    statoAttuale = StatoBoss.Attacco;
                }
                break;

            case StatoBoss.Fuga:
                if (distanza >= raggioAttacco) 
                {
                    statoAttuale = StatoBoss.Attacco; 
                }
                else if (distanza >= raggioFuga + 1f) 
                {
                    statoAttuale = StatoBoss.Avvicinamento;
                }
                else
                {
                    MuovitiLontanoDalGiocatore();
                }
                break;

            case StatoBoss.Attacco:
                if (stileDiCombattimento == TipoBoss.Distanza && distanza < raggioFuga)
                {
                    statoAttuale = StatoBoss.Fuga;
                    break;
                }
                if (distanza > raggioAttacco)
                {
                    statoAttuale = StatoBoss.Avvicinamento;
                    break;
                }

                if (stileDiCombattimento == TipoBoss.CorpoACorpo)
                {
                    EseguiAttaccoMelee();
                }
                else
                {
                    EseguiAttaccoDistanza();
                }

                timer = tempoTraAttacchi;
                statoAttuale = StatoBoss.Recupero;
                break;

            case StatoBoss.Recupero:
                if (stileDiCombattimento == TipoBoss.Distanza && distanza < raggioFuga)
                {
                    statoAttuale = StatoBoss.Fuga;
                }

                timer -= Time.deltaTime;
                if (timer <= 0 && statoAttuale == StatoBoss.Recupero)
                {
                    statoAttuale = StatoBoss.Avvicinamento;
                }
                break;
        }
    }

    void MuovitiVersoGiocatore()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) {
            float dirX = Mathf.Sign(giocatore.position.x - transform.position.x);
            rb.velocity = new Vector2(dirX * velocita, rb.velocity.y);
        } else {
            Vector2 destinazione = new Vector2(giocatore.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, destinazione, velocita * Time.deltaTime);
        }
    }

    void MuovitiLontanoDalGiocatore()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) {
            float dirX = Mathf.Sign(transform.position.x - giocatore.position.x);
            rb.velocity = new Vector2(dirX * velocita, rb.velocity.y);
        } else {
            float dirX = transform.position.x - giocatore.position.x;
            Vector2 direzione = new Vector2(Mathf.Sign(dirX), 0);
            transform.Translate(direzione * velocita * Time.deltaTime);
        }
    }

    void EseguiAttaccoMelee()
    {
        if (animator != null) animator.SetTrigger("Attacco");

        IDamageable target = giocatore.GetComponent<IDamageable>();
        if (target != null)
        {
            target.PrendiDanno(15f);
        }
    }

    void EseguiAttaccoDistanza()
    {
        if (animator != null) animator.SetTrigger("AttaccoDistanza");

        if (proiettilePrefab != null && puntoSparo != null)
        {
            GameObject colpo = Instantiate(proiettilePrefab, puntoSparo.position, Quaternion.identity);
            Proiettile scriptProiettile = colpo.GetComponent<Proiettile>();

            if (scriptProiettile != null)
            {
                float direzione = Mathf.Sign(transform.localScale.x);
                scriptProiettile.Lancia(direzione, gameObject);
            }
        }
    }

    void GiraVersoAvversario()
    {
        float scaleX = Mathf.Abs(transform.localScale.x);
        float scaleY = transform.localScale.y;
        float scaleZ = transform.localScale.z;

        if (giocatore.position.x > transform.position.x)
            transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        else
            transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
    }
}
