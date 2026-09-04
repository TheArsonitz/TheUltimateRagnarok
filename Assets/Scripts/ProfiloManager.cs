using UnityEngine;
using UnityEngine.UI;

public class ProfiloManager : MonoBehaviour
{
    [Header("Pannelli UI Profilo")]
    public GameObject pannelloSceltaProfilo;
    public GameObject pannelloNuovoNome;
    public GameObject pannelloTutorial;
    public GameObject pannelloLivelli;
    public GameObject menuSelezione;

    [Header("Elementi Scelta Profilo")]
    public Text testoNomeGiocatore;
    public Text testoLivelloRaggiunto;

    [Header("Elementi Nuovo Nome")]
    public InputField inputNome;

    void OnEnable()
    {
        AggiornaSchermata();
    }

    public void AvviaFlussoPvE()
    {
        // Rendi visibile questo pannello (PannelloProfilo)
        gameObject.SetActive(true);

        if (menuSelezione != null) menuSelezione.SetActive(false);

        AggiornaSchermata();
    }

    public void AggiornaSchermata()
    {
        string nomeSalvato = PlayerPrefs.GetString("PlayerName", "");

        if (!string.IsNullOrEmpty(nomeSalvato))
        {
            // C'e' gia' un profilo esistente: chiediamo se continuare o ricominciare
            if (pannelloSceltaProfilo != null) pannelloSceltaProfilo.SetActive(true);
            if (pannelloNuovoNome != null) pannelloNuovoNome.SetActive(false);

            if (testoNomeGiocatore != null)
                testoNomeGiocatore.text = "PROFILO ATTIVO: " + nomeSalvato.ToUpper();

            int livMax = PlayerPrefs.GetInt("LivelloMaxSbloccato", 1);
            if (testoLivelloRaggiunto != null)
                testoLivelloRaggiunto.text = "LIVELLO RAGGIUNTO: " + livMax;
        }
        else
        {
            // Primo avvio: chiediamo subito il nome
            MostraInserimentoNuovoNome();
        }
    }

    public void ContinuaPartita()
    {
        gameObject.SetActive(false);
        if (pannelloSceltaProfilo != null) pannelloSceltaProfilo.SetActive(false);
        if (pannelloNuovoNome != null) pannelloNuovoNome.SetActive(false);

        if (pannelloLivelli != null)
        {
            pannelloLivelli.SetActive(true);
        }
    }

    public void MostraInserimentoNuovoNome()
    {
        gameObject.SetActive(true);
        if (pannelloSceltaProfilo != null) pannelloSceltaProfilo.SetActive(false);
        if (pannelloNuovoNome != null) pannelloNuovoNome.SetActive(true);

        if (inputNome != null)
        {
            inputNome.text = "";
            inputNome.ActivateInputField();
        }
    }

    public void ConfermaNuovoNome()
    {
        string nomeScelto = (inputNome != null && !string.IsNullOrEmpty(inputNome.text.Trim())) 
            ? inputNome.text.Trim() 
            : "Eroe";

        PlayerPrefs.SetString("PlayerName", nomeScelto);
        // Resettiamo il percorso al Livello 1 per la nuova partita con questo nome
        PlayerPrefs.SetInt("LivelloMaxSbloccato", 1);
        PlayerPrefs.Save();

        gameObject.SetActive(false);
        if (pannelloNuovoNome != null) pannelloNuovoNome.SetActive(false);
        if (pannelloSceltaProfilo != null) pannelloSceltaProfilo.SetActive(false);

        // Avviamo il tutorial narrativo con il nome del nuovo giocatore
        if (pannelloTutorial != null)
        {
            pannelloTutorial.SetActive(true);
            var tutUI = pannelloTutorial.GetComponent<TutorialStoriaUI>();
            if (tutUI != null)
            {
                tutUI.AggiornaMessaggioPersonalizzato();
            }
        }
        else if (pannelloLivelli != null)
        {
            pannelloLivelli.SetActive(true);
        }
    }

    public void TornaASelezione()
    {
        gameObject.SetActive(false);
        if (pannelloSceltaProfilo != null) pannelloSceltaProfilo.SetActive(false);
        if (pannelloNuovoNome != null) pannelloNuovoNome.SetActive(false);
        if (menuSelezione != null) menuSelezione.SetActive(true);
    }
}
