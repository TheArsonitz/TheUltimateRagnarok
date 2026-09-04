using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Livelli")]
    public int livelloAttuale = 1;

    [Header("Modalita'")]
    public bool modalitaPvE = true;

    [Header("Impostazioni Base")]
    public float durataPartitaSecondi = 180f;

    [Header("Riferimenti Giocatori")]
    public GameObject player1;
    public GameObject player2;

    [Header("Interfaccia (UI)")]
    public Text testoTimer;        
    public Text testoVittoria;     
    public GameObject pannelloFine;

    private float tempoRimanente;
    private bool partitaInCorso = true;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
       
        tempoRimanente = durataPartitaSecondi;
        partitaInCorso = true;

        
        Time.timeScale = 1;
        if (pannelloFine != null) pannelloFine.SetActive(false);
    }

    void Update()
    {
        if (partitaInCorso)
        {
            
            tempoRimanente -= Time.deltaTime;

            
            AggiornaGraficaTimer();

            
            if (tempoRimanente <= 0)
            {
                tempoRimanente = 0;
                FinePerTempo();
            }
        }
    }

    
    void FinePerTempo()
    {
        partitaInCorso = false;

       
        float vitaP1 = player1.GetComponent<HealthSystem>().vitaAttuale;
        float vitaP2 = player2.GetComponent<HealthSystem>().vitaAttuale;

        string messaggio = "";

        if (vitaP1 > vitaP2)
        {
            messaggio = "TEMPO SCADUTO!\nVINCE PLAYER 1 (Per Vita)";
        }
        else if (vitaP2 > vitaP1)
        {
            messaggio = "TEMPO SCADUTO!\nVINCE PLAYER 2 (Per Vita)";
        }
        else
        {
            messaggio = "TEMPO SCADUTO!\nPAREGGIO PERFETTO!";
        }

        MostraSchermataFinale(messaggio);
    }

    public void GiocatoreMorto(string nomeSconfitto)
    {
        if (!partitaInCorso) return;

        partitaInCorso = false;
        string messaggio = "";

        if (nomeSconfitto == player1.name)
        {
            messaggio = "K.O.!\nVINCE PLAYER 2";
        }
        else
        {
            messaggio = "K.O.!\nVINCE PLAYER 1";

            if (modalitaPvE)
            {
                float tempoImpiegato = durataPartitaSecondi - tempoRimanente;
                string nomeGiocatore = PlayerPrefs.GetString("PlayerName", "Eroe");
                ClassificaManager.SalvaTempo(livelloAttuale, nomeGiocatore, tempoImpiegato);

                int livelloMaxSbloccato = PlayerPrefs.GetInt("LivelloMaxSbloccato", 1);
                if (livelloAttuale >= livelloMaxSbloccato)
                {
                    PlayerPrefs.SetInt("LivelloMaxSbloccato", livelloAttuale + 1);
                    PlayerPrefs.Save();
                    Debug.Log("Hai sbloccato il livello " + (livelloAttuale + 1) + "!");
                }
            }
        }

        MostraSchermataFinale(messaggio);
    }

    void AggiornaGraficaTimer()
    {
        if (testoTimer != null)
        {
            float minuti = Mathf.FloorToInt(tempoRimanente / 60);
            float secondi = Mathf.FloorToInt(tempoRimanente % 60);
            testoTimer.text = string.Format("{0:00}:{1:00}", minuti, secondi);

           
            if (tempoRimanente <= 10) testoTimer.color = Color.red;
            else testoTimer.color = Color.white;
        }
    }

    void MostraSchermataFinale(string testo)
    {
        Debug.Log(testo); 

        if (testoVittoria != null) testoVittoria.text = testo;
        if (pannelloFine != null) pannelloFine.SetActive(true);

        Time.timeScale = 0; 

        StartCoroutine(AttendiERiavvia());
    }

    System.Collections.IEnumerator AttendiERiavvia()
    {
        yield return new WaitForSecondsRealtime(3f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

