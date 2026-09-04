using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public enum TipoComandi { WASD, Tastierino }

    [Header("Pannelli UI (Opzionali per Menu Principale)")]
    public GameObject pannelloSettings;
    public GameObject pannelloCrediti;
    public GameObject pannelloSelezione;
    public GameObject pannelloLivelli;

    [Header("Slider Volume")]
    public Image immagineFillSlider;
    public Color coloreVolumeBasso = new Color(1f, 0.8f, 0.6f);
    public Color coloreVolumeAlto = new Color(1f, 0.45f, 0f);
    public Slider sliderVolumePrincipale;

    [Header("Impostazioni Risoluzione & Schermo")]
    public Dropdown menuRisoluzione;
    private Resolution[] risoluzioniSupportate;
    public Toggle toggleFullScreen;

    [Header("Impostazioni Comandi")]
    public Dropdown dropdownComandiP1;
    public Text testoComandiP1;
    public Dropdown dropdownComandiP2;
    public Text testoComandiP2;

    private string setWASD = "Jump: W - Left: A \nRight: D - Attack: Space \nCollect: E - Protect: S";
    private string setTastierino = "Jump: [8] - Left: [4] \nRight: [6] - Attack: [0] \nCollect: [7] - Protect: [5]";

    void Awake()
    {
        // Assicura che il tempo sia sempre attivo nel menu
        Time.timeScale = 1f;
        // Applica subito le preferenze salvate all'avvio della scena
        InizializzaImpostazioni();

        if (sliderVolumePrincipale != null)
        {
            sliderVolumePrincipale.onValueChanged.RemoveListener(ImpostaVolume);
            sliderVolumePrincipale.onValueChanged.AddListener(ImpostaVolume);
        }
        if (toggleFullScreen != null)
        {
            toggleFullScreen.onValueChanged.RemoveListener(ImpostaSchermoIntero);
            toggleFullScreen.onValueChanged.AddListener(ImpostaSchermoIntero);
        }
        if (menuRisoluzione != null)
        {
            menuRisoluzione.onValueChanged.RemoveListener(ImpostaRisoluzione);
            menuRisoluzione.onValueChanged.AddListener(ImpostaRisoluzione);
        }
    }

    void Start()
    {
        // Se usato nel MainMenu, chiude i pannelli secondari.
        // Se attaccato a un pannello di pausa, evita di disattivare il proprio GameObject.
        if (pannelloSettings != null && pannelloSettings != gameObject) pannelloSettings.SetActive(false);
        if (pannelloCrediti != null && pannelloCrediti != gameObject) pannelloCrediti.SetActive(false);
        if (pannelloSelezione != null && pannelloSelezione != gameObject) pannelloSelezione.SetActive(false);
        if (pannelloLivelli != null && pannelloLivelli != gameObject) pannelloLivelli.SetActive(false);

        SincronizzaUI();

        if (dropdownComandiP1 != null) CambiaComandiP1(dropdownComandiP1.value);
        if (dropdownComandiP2 != null) CambiaComandiP2(dropdownComandiP2.value);

        if (PlayerPrefs.GetInt("RitornoDaPvE", 0) == 1)
        {
            PlayerPrefs.SetInt("RitornoDaPvE", 0);
            PlayerPrefs.Save();
            ApriMenuLivelli(); // Apre direttamente la mappa livelli
        }
    }

    void OnEnable()
    {
        // Quando il pannello viene aperto/attivato (es. aprendo il menu di pausa), aggiorna la grafica dei controlli
        SincronizzaUI();
    }

    public void InizializzaImpostazioni()
    {
        // 1. Carica e applica il Volume (supporta sia VolumeMaster che VolumeGioco per retrocompatibilità)
        float volumeSalvato = PlayerPrefs.GetFloat("VolumeMaster", PlayerPrefs.GetFloat("VolumeGioco", 1f));
        AudioListener.volume = Mathf.Pow(volumeSalvato, 2f);

        // 2. Carica e applica Schermo Intero
        if (PlayerPrefs.HasKey("FullScreen"))
        {
            bool isFull = PlayerPrefs.GetInt("FullScreen", Screen.fullScreen ? 1 : 0) == 1;
            Screen.fullScreen = isFull;
        }

        // 3. Risoluzioni disponibili del monitor
        risoluzioniSupportate = Screen.resolutions;

        // 4. Carica e applica la Risoluzione salvata
        int indiceRisoluzioneSalvata = PlayerPrefs.GetInt("IndiceRisoluzione", -1);
        if (risoluzioniSupportate != null && indiceRisoluzioneSalvata >= 0 && indiceRisoluzioneSalvata < risoluzioniSupportate.Length)
        {
            Resolution r = risoluzioniSupportate[indiceRisoluzioneSalvata];
            Screen.SetResolution(r.width, r.height, Screen.fullScreen);
        }
    }

    public void SincronizzaUI()
    {
        // Sincronizza lo slider del volume
        float volumeSalvato = PlayerPrefs.GetFloat("VolumeMaster", PlayerPrefs.GetFloat("VolumeGioco", 1f));
        if (sliderVolumePrincipale != null)
        {
            sliderVolumePrincipale.value = volumeSalvato;
        }
        if (immagineFillSlider != null)
        {
            immagineFillSlider.color = Color.Lerp(coloreVolumeBasso, coloreVolumeAlto, volumeSalvato);
        }

        // Sincronizza il toggle di schermo intero
        if (toggleFullScreen != null)
        {
            bool isFull = PlayerPrefs.GetInt("FullScreen", Screen.fullScreen ? 1 : 0) == 1;
            toggleFullScreen.isOn = isFull;
        }

        // Sincronizza il dropdown delle risoluzioni
        if (risoluzioniSupportate == null || risoluzioniSupportate.Length == 0)
        {
            risoluzioniSupportate = Screen.resolutions;
        }

        if (menuRisoluzione != null && risoluzioniSupportate != null && risoluzioniSupportate.Length > 0)
        {
            menuRisoluzione.ClearOptions();
            List<string> opzioni = new List<string>();
            int indiceSelezionato = 0;
            int indiceSalvato = PlayerPrefs.GetInt("IndiceRisoluzione", -1);

            for (int i = 0; i < risoluzioniSupportate.Length; i++)
            {
                string opzione = risoluzioniSupportate[i].width + "x" + risoluzioniSupportate[i].height;
                opzioni.Add(opzione);

                if (indiceSalvato == i)
                {
                    indiceSelezionato = i;
                }
                else if (indiceSalvato == -1 &&
                         risoluzioniSupportate[i].width == Screen.currentResolution.width &&
                         risoluzioniSupportate[i].height == Screen.currentResolution.height)
                {
                    indiceSelezionato = i;
                }
            }

            menuRisoluzione.AddOptions(opzioni);
            menuRisoluzione.value = indiceSelezionato;
            menuRisoluzione.RefreshShownValue();
        }
    }

    // ==========================================
    // METODI PUBBLICI PER I COMPONENTI UI
    // ==========================================

    /// <summary>
    /// Da assegnare all'evento OnValueChanged di uno Slider UI.
    /// Ha effetto immediato su AudioListener e salva su PlayerPrefs.
    /// </summary>
    public void ImpostaVolume(float volume)
    {
        AudioListener.volume = Mathf.Pow(volume, 2f);
        if (immagineFillSlider != null)
            immagineFillSlider.color = Color.Lerp(coloreVolumeBasso, coloreVolumeAlto, volume);

        PlayerPrefs.SetFloat("VolumeMaster", volume);
        PlayerPrefs.SetFloat("VolumeGioco", volume);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Da assegnare all'evento OnValueChanged di un Toggle UI.
    /// Ha effetto immediato sullo schermo e salva su PlayerPrefs.
    /// </summary>
    public void ImpostaSchermoIntero(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("FullScreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Da assegnare all'evento OnValueChanged di un Dropdown UI.
    /// Ha effetto immediato sulla risoluzione e salva su PlayerPrefs.
    /// </summary>
    public void ImpostaRisoluzione(int indiceRisoluzione)
    {
        if (risoluzioniSupportate == null || risoluzioniSupportate.Length == 0)
            risoluzioniSupportate = Screen.resolutions;

        if (risoluzioniSupportate != null && indiceRisoluzione >= 0 && indiceRisoluzione < risoluzioniSupportate.Length)
        {
            Resolution risoluzioneScelta = risoluzioniSupportate[indiceRisoluzione];
            Screen.SetResolution(risoluzioneScelta.width, risoluzioneScelta.height, Screen.fullScreen);
            PlayerPrefs.SetInt("IndiceRisoluzione", indiceRisoluzione);
            PlayerPrefs.Save();
        }
    }

    // Alias per retrocompatibilità con i componenti già configurati nel MainMenu
    public void CambiaVolume(float valoreVolume) => ImpostaVolume(valoreVolume);
    public void CambiaSchermoIntero(bool isFullScreen) => ImpostaSchermoIntero(isFullScreen);
    public void CambiaRisoluzione(int indiceRisoluzione) => ImpostaRisoluzione(indiceRisoluzione);

    // ==========================================
    // GESTIONE PANNELLI DEL MAIN MENU
    // ==========================================

    public void ApriMenuSelezione()
    {
        if (pannelloSelezione != null) pannelloSelezione.SetActive(true);
    }
    public void ChiudiMenuSelezione()
    {
        if (pannelloSelezione != null) pannelloSelezione.SetActive(false);
    }

    public void ApriMenuLivelli()
    {
        if (pannelloSelezione != null) pannelloSelezione.SetActive(false);
        if (pannelloLivelli != null) pannelloLivelli.SetActive(true);
    }
    public void ChiudiMenuLivelli()
    {
        if (pannelloLivelli != null) pannelloLivelli.SetActive(false);
        if (pannelloSelezione != null) pannelloSelezione.SetActive(true);
    }

    public void ApriImpostazioni()
    {
        if (pannelloSettings != null) pannelloSettings.SetActive(true);
    }
    public void ChiudiImpostazioni()
    {
        if (pannelloSettings != null) pannelloSettings.SetActive(false);
    }

    public void ApriCrediti()
    {
        if (pannelloCrediti != null) pannelloCrediti.SetActive(true);
    }
    public void ChiudiCrediti()
    {
        if (pannelloCrediti != null) pannelloCrediti.SetActive(false);
    }

    // ==========================================
    // GESTIONE COMANDI
    // ==========================================

    public void CambiaComandiP1(int indiceSet)
    {
        PlayerPrefs.SetInt("SetComandiP1", indiceSet);
        PlayerPrefs.Save();

        TipoComandi tipoScelto = (TipoComandi)indiceSet;
        if (testoComandiP1 != null)
        {
            if (tipoScelto == TipoComandi.WASD) testoComandiP1.text = setWASD;
            else if (tipoScelto == TipoComandi.Tastierino) testoComandiP1.text = setTastierino;
        }
        if (dropdownComandiP2 != null && dropdownComandiP2.value == indiceSet)
        {
            dropdownComandiP2.value = (indiceSet == 0) ? 1 : 0;
        }

        AggiornaGiocatoriInScena();
    }

    public void CambiaComandiP2(int indiceSet)
    {
        TipoComandi tipoScelto = (TipoComandi)indiceSet;
        if (testoComandiP2 != null)
        {
            if (tipoScelto == TipoComandi.WASD) testoComandiP2.text = setWASD;
            else if (tipoScelto == TipoComandi.Tastierino) testoComandiP2.text = setTastierino;
        }
        if (dropdownComandiP1 != null && dropdownComandiP1.value == indiceSet)
        {
            dropdownComandiP1.value = (indiceSet == 0) ? 1 : 0;
        }
    }

    public static void AggiornaGiocatoriInScena()
    {
        var movements = FindObjectsOfType<PlayerMovement>();
        foreach (var m in movements) if (m != null) m.AggiornaComandiDaPrefs();

        var attacks = FindObjectsOfType<PlayerAttack>();
        foreach (var a in attacks) if (a != null) a.AggiornaComandiDaPrefs();

        var defenses = FindObjectsOfType<PlayerDefense>();
        foreach (var d in defenses) if (d != null) d.AggiornaComandiDaPrefs();
    }
}