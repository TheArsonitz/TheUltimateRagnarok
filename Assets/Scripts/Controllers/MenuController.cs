using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    [Header("Pannelli")]
    public GameObject pannelloSettings;

    [Header("Slider Volume")]
    public Image immagineFillSlider;
    public Color coloreVolumeBasso = new Color(1f, 0.8f, 0.6f);
    public Color coloreVolumeAlto = new Color(1f, 0.45f, 0f);
    public Slider sliderVolumePrincipale;

    [Header("Impostazioni Risoluzione")]
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

    void Start()
    {
        if (pannelloSettings != null)
            pannelloSettings.SetActive(false);

        if (sliderVolumePrincipale != null)
            CambiaVolume(sliderVolumePrincipale.value);

        risoluzioniSupportate = Screen.resolutions;

        if (menuRisoluzione != null) {

            menuRisoluzione.ClearOptions();

            List<string> opzioni = new List<string>();
            int indiceRisoluzioneAttuale = 0;

            for (int i = 0; i < risoluzioniSupportate.Length; i++) {

                string opzione = risoluzioniSupportate[i].width + "x" + risoluzioniSupportate[i].height;
                opzioni.Add(opzione);

                if (risoluzioniSupportate[i].width == Screen.currentResolution.width && risoluzioniSupportate[i].height == Screen.currentResolution.height)
                    indiceRisoluzioneAttuale = i;

            }

            menuRisoluzione.AddOptions(opzioni);
            menuRisoluzione.value = indiceRisoluzioneAttuale;
            menuRisoluzione.RefreshShownValue();

        }

        if (toggleFullScreen != null)
            toggleFullScreen.isOn = Screen.fullScreen;

        if (dropdownComandiP1 != null) CambiaComandiP1(dropdownComandiP1.value);
        if (dropdownComandiP2 != null) CambiaComandiP2(dropdownComandiP2.value);

    }

    public void ModalitaGioco() {
        SceneManager.LoadScene("Modalita' di Gioco");
    }

    public void ApriImpostazioni() {
        pannelloSettings.SetActive(true);
    }

    public void ChiudiImpostazioni() {
        pannelloSettings.SetActive(false);
    }

    public void CambiaVolume(float valoreVolume) {

        AudioListener.volume = Mathf.Pow(valoreVolume, 2f);

        if (immagineFillSlider != null)
            immagineFillSlider.color = Color.Lerp(coloreVolumeBasso,
                                        coloreVolumeAlto, valoreVolume);

    }

    public void CambiaRisoluzione(int indiceRisoluzione) {

        Resolution risoluzioneScelta = risoluzioniSupportate[indiceRisoluzione];
        Screen.SetResolution(risoluzioneScelta.width, risoluzioneScelta.height, Screen.fullScreen);

    }

    public void CambiaSchermoIntero(bool isFullScreen) {
        Screen.fullScreen = isFullScreen;
    }

    public void CambiaComandiP1(int indiceSet) {
        if (testoComandiP1 != null) {
            if (indiceSet == 0) testoComandiP1.text = setWASD;
            else if (indiceSet == 1) testoComandiP1.text = setTastierino;
        }

        if (dropdownComandiP2 != null && dropdownComandiP2.value == indiceSet) {
            int nuovoIndiceP2 = (indiceSet == 0) ? 1 : 0;   
            dropdownComandiP2.value = nuovoIndiceP2;
        }

    }

    public void CambiaComandiP2(int indiceSet) {
        if (testoComandiP2 != null)
        {
            if (indiceSet == 0) testoComandiP2.text = setWASD;
            else if (indiceSet == 1) testoComandiP2.text = setTastierino;
        }

        if (dropdownComandiP1 != null && dropdownComandiP1.value == indiceSet)
        {
            int nuovoIndiceP1 = (indiceSet == 0) ? 1 : 0;
            dropdownComandiP1.value = nuovoIndiceP1;
        }
    }

}
