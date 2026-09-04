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
    public float raggioAttacco = 1.5f; // Per il Corpo a Corpo è la hitbox, per la Distanza è il range di tiro
    public float tempoTraAttacchi = 2f;
    
    [Header("Solo per Combattimento a Distanza")]
    public float raggioFuga = 4f; // Se il giocatore si avvicina più di questo valore, il boss indietreggia

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
        if (giocatore == null) return;

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
                    statoAttuale = StatoBoss.Attacco; // Abbastanza lontano da sparare
                }
                else if (distanza >= raggioFuga + 1f) // Un po' di margine per non oscillare
                {
                    statoAttuale = StatoBoss.Avvicinamento;
                }
                else
                {
                    MuovitiLontanoDalGiocatore();
                }
                break;

            case StatoBoss.Attacco:
                // Controlla che non sia cambiato lo stato di sicurezza nel frattempo (se a distanza)
                if (stileDiCombattimento == TipoBoss.Distanza && distanza < raggioFuga)
                {
                    statoAttuale = StatoBoss.Fuga;
                    break;
                }
                // Se il bersaglio esce dal range, torna a inseguirlo
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
                // Anche in recupero, se a distanza, fuggiamo se si avvicina troppo
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
        Vector2 destinazione = new Vector2(giocatore.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, destinazione, velocita * Time.deltaTime);
    }

    void MuovitiLontanoDalGiocatore()
    {
        // Trova la direzione opposta
        float dirX = transform.position.x - giocatore.position.x;
        Vector2 direzione = new Vector2(Mathf.Sign(dirX), 0);
        transform.Translate(direzione * velocita * Time.deltaTime);
    }

    void EseguiAttaccoMelee()
    {
        if (animator != null) animator.SetTrigger("Attacco");

        HealthSystem vitaDelGiocatore = giocatore.GetComponent<HealthSystem>();
        if (vitaDelGiocatore != null)
        {
            vitaDelGiocatore.PrendiDanno(15f);
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
        if (giocatore.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }
}