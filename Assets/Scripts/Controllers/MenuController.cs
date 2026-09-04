using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public enum TipoComandi { WASD, Tastierino }

    [Header("Pannelli UI")]
    public GameObject pannelloSettings;
    public GameObject pannelloCrediti;
    public GameObject pannelloSelezione;
    public GameObject pannelloLivelli;

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
        if (pannelloSettings != null) pannelloSettings.SetActive(false);
        if (pannelloCrediti != null) pannelloCrediti.SetActive(false);
        if (pannelloSelezione != null) pannelloSelezione.SetActive(false);
        if (pannelloLivelli != null) pannelloLivelli.SetActive(false);

        if (sliderVolumePrincipale != null)
        {
            float volumeSalvato = PlayerPrefs.GetFloat("VolumeGioco", 1f);
            sliderVolumePrincipale.value = volumeSalvato;
            CambiaVolume(volumeSalvato);
        }

        risoluzioniSupportate = Screen.resolutions;
        if (menuRisoluzione != null)
        {
            menuRisoluzione.ClearOptions();
            List<string> opzioni = new List<string>();
            int indiceRisoluzioneAttuale = 0;

            for (int i = 0; i < risoluzioniSupportate.Length; i++)
            {
                string opzione = risoluzioniSupportate[i].width + "x" + risoluzioniSupportate[i].height;
                opzioni.Add(opzione);
                if (risoluzioniSupportate[i].width == Screen.currentResolution.width && risoluzioniSupportate[i].height == Screen.currentResolution.height)
                    indiceRisoluzioneAttuale = i;
            }
            menuRisoluzione.AddOptions(opzioni);
            menuRisoluzione.value = indiceRisoluzioneAttuale;
            menuRisoluzione.RefreshShownValue();
        }

        if (toggleFullScreen != null) toggleFullScreen.isOn = Screen.fullScreen;
        if (dropdownComandiP1 != null) CambiaComandiP1(dropdownComandiP1.value);
        if (dropdownComandiP2 != null) CambiaComandiP2(dropdownComandiP2.value);
    }


    public void ApriMenuSelezione()
    {
        pannelloSelezione.SetActive(true);
    }
    public void ChiudiMenuSelezione()
    {
        pannelloSelezione.SetActive(false);
    }

    public void ApriMenuLivelli()
    {
        pannelloSelezione.SetActive(false);
        pannelloLivelli.SetActive(true);
    }
    public void ChiudiMenuLivelli()
    {
        pannelloLivelli.SetActive(false);
        pannelloSelezione.SetActive(true);
    }

    public void ApriImpostazioni()
    {
        pannelloSettings.SetActive(true);
    }
    public void ChiudiImpostazioni()
    {
        pannelloSettings.SetActive(false);
    }

    public void ApriCrediti()
    {
        pannelloCrediti.SetActive(true);
    }
    public void ChiudiCrediti()
    {
        pannelloCrediti.SetActive(false);
    }

    public void CambiaVolume(float valoreVolume)
    {
        AudioListener.volume = Mathf.Pow(valoreVolume, 2f);
        if (immagineFillSlider != null)
            immagineFillSlider.color = Color.Lerp(coloreVolumeBasso, coloreVolumeAlto, valoreVolume);

        PlayerPrefs.SetFloat("VolumeGioco", valoreVolume);
        PlayerPrefs.Save();
    }

    public void CambiaRisoluzione(int indiceRisoluzione)
    {
        Resolution risoluzioneScelta = risoluzioniSupportate[indiceRisoluzione];
        Screen.SetResolution(risoluzioneScelta.width, risoluzioneScelta.height, Screen.fullScreen);
    }

    public void CambiaSchermoIntero(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void CambiaComandiP1(int indiceSet)
    {
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
}