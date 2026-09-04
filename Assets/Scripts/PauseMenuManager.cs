using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Pannelli UI Pausa")]
    public GameObject pannelloPausa;
    public GameObject pannelloTutorial;
    public GameObject pannelloImpostazioni;

    private bool isPausaAttiva = false;

    void Start()
    {
        if (pannelloPausa != null) pannelloPausa.SetActive(false);
        if (pannelloTutorial != null) pannelloTutorial.SetActive(false);
        if (pannelloImpostazioni != null) pannelloImpostazioni.SetActive(false);
        isPausaAttiva = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPausaAttiva)
            {
                AttivaPausa();
            }
            else
            {
                RiprendiGioco();
            }
        }
    }

    private void AttivaPausa()
    {
        Time.timeScale = 0f;
        if (pannelloPausa != null) pannelloPausa.SetActive(true);
        if (pannelloTutorial != null) pannelloTutorial.SetActive(false);
        if (pannelloImpostazioni != null) pannelloImpostazioni.SetActive(false);
        isPausaAttiva = true;
    }

    public void RiprendiGioco()
    {
        Time.timeScale = 1f;
        if (pannelloPausa != null) pannelloPausa.SetActive(false);
        if (pannelloTutorial != null) pannelloTutorial.SetActive(false);
        if (pannelloImpostazioni != null) pannelloImpostazioni.SetActive(false);
        isPausaAttiva = false;
    }

    public void ApriTutorial()
    {
        if (pannelloPausa != null) pannelloPausa.SetActive(false);
        if (pannelloTutorial != null) pannelloTutorial.SetActive(true);
    }

    public void ChiudiTutorial()
    {
        if (pannelloTutorial != null) pannelloTutorial.SetActive(false);
        if (pannelloPausa != null) pannelloPausa.SetActive(true);
    }

    public void ApriImpostazioni()
    {
        if (pannelloPausa != null) pannelloPausa.SetActive(false);
        if (pannelloImpostazioni != null) pannelloImpostazioni.SetActive(true);
    }

    public void ChiudiImpostazioni()
    {
        if (pannelloImpostazioni != null) pannelloImpostazioni.SetActive(false);
        if (pannelloPausa != null) pannelloPausa.SetActive(true);
    }

    public void EsciAlMenuPrincipale()
    {
        Time.timeScale = 1f;
        isPausaAttiva = false;

        PlayerPrefs.SetInt("RitornoDaPvE", 1);
        PlayerPrefs.Save();

        LevelLoader.CaricaMenuPrincipale();
    }
}
