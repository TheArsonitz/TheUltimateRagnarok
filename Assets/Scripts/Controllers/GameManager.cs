using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Impostazioni Livello")]
    public int livelloAttuale = 1;
    public float tempoIniziale = 180f;
    private float tempoAttuale;

    [Header("Riferimenti Giocatori")]
    public GameObject player1;
    public GameObject player2;

    [Header("Interfaccia (UI)")]
    public GameObject pannelloVittoria;
    public GameObject pannelloSconfitta;
    public Text testoTimer;

    private bool partitaInCorso = true;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        partitaInCorso = true;
        Time.timeScale = 1;
        tempoAttuale = tempoIniziale;
        
        if (pannelloVittoria != null) pannelloVittoria.SetActive(false);
        if (pannelloSconfitta != null) pannelloSconfitta.SetActive(false);

        if (player1 != null)
        {
            HealthSystem hs1 = player1.GetComponent<HealthSystem>();
            if (hs1 != null) hs1.OnDeath += GestisciMorte;
        }

        if (player2 != null)
        {
            HealthSystem hs2 = player2.GetComponent<HealthSystem>();
            if (hs2 != null) hs2.OnDeath += GestisciMorte;
        }
    }

    void Update()
    {
        if (partitaInCorso)
        {
            tempoAttuale -= Time.deltaTime;
            if (tempoAttuale <= 0)
            {
                tempoAttuale = 0;
                partitaInCorso = false;
                StartCoroutine(RoutineSconfitta());
            }

            if (testoTimer != null)
            {
                int m = Mathf.FloorToInt(tempoAttuale / 60);
                int s = Mathf.FloorToInt(tempoAttuale % 60);
                testoTimer.text = string.Format("{0:00}:{1:00}", m, s);
                testoTimer.color = tempoAttuale <= 10f ? Color.red : Color.white;
            }
        }
    }

    void OnDestroy()
    {
        if (player1 != null)
        {
            HealthSystem hs1 = player1.GetComponent<HealthSystem>();
            if (hs1 != null) hs1.OnDeath -= GestisciMorte;
        }
        if (player2 != null)
        {
            HealthSystem hs2 = player2.GetComponent<HealthSystem>();
            if (hs2 != null) hs2.OnDeath -= GestisciMorte;
        }
    }

    private void GestisciMorte(GameObject sconfitto)
    {
        if (!partitaInCorso) return;
        partitaInCorso = false;

        if (sconfitto == player1)
        {
            StartCoroutine(RoutineSconfitta());
        }
        else if (sconfitto == player2)
        {
            StartCoroutine(RoutineVittoria());
        }
    }

    private IEnumerator RoutineSconfitta()
    {
        yield return new WaitForSeconds(2f);
        if (pannelloSconfitta != null) pannelloSconfitta.SetActive(true);
        Time.timeScale = 0; 
    }

    private IEnumerator RoutineVittoria()
    {
        yield return new WaitForSeconds(2f);

        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        int livelloMaxSbloccato = PlayerPrefs.GetInt("LivelloMaxSbloccato", 1);
        
        // Evitiamo che sblocchi il Livello 6 (che non esiste)
        if (currentLevel >= livelloMaxSbloccato && currentLevel < 5)
        {
            PlayerPrefs.SetInt("LivelloMaxSbloccato", currentLevel + 1);
        }

        // Salva il tempo in classifica (recupera in automatico il nome)
        float tempoImpiegato = tempoIniziale - tempoAttuale;
        ClassificaManager.SalvaTempo(currentLevel, tempoImpiegato);

        if (pannelloVittoria != null) pannelloVittoria.SetActive(true);
        Time.timeScale = 0;
    }

    public void RiavviaLivello()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ProssimoLivello()
    {
        Time.timeScale = 1;
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        
        // Se siamo al livello 5, non c'è un "Prossimo Livello", quindi forziamo il ritorno al menu
        if (currentIndex < 5)
        {
            SceneManager.LoadScene(currentIndex + 1);
        }
        else
        {
            EsciAlMenu();
        }
    }

    public void EsciAlMenu()
    {
        Time.timeScale = 1;
        // Permette al menu di riaprirsi direttamente nella schermata Livelli
        PlayerPrefs.SetInt("RitornoDaPvE", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);
    }
}
