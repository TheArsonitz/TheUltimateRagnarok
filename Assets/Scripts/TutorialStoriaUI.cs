using UnityEngine;
using UnityEngine.UI;

public class TutorialStoriaUI : MonoBehaviour
{
    [Header("Slide del Tutorial")]
    public GameObject[] slides;
    public Text testoMessaggioPersonalizzato;

    [Header("Pulsanti Navigazione")]
    public Button bottonePrecedente;
    public Button bottoneSuccessivo;
    public Button bottoneIniziaGioco;
    public Button bottoneSaltaTutorial;

    [Header("Riferimento Mappa Livelli")]
    public GameObject pannelloLivelli;

    private int indiceSlideAttuale = 0;

    void OnEnable()
    {
        indiceSlideAttuale = 0;
        AggiornaMessaggioPersonalizzato();
        AggiornaVisualizzazioneSlide();
    }

    public void AggiornaMessaggioPersonalizzato()
    {
        string nome = PlayerPrefs.GetString("PlayerName", "Eroe");
        if (testoMessaggioPersonalizzato != null)
        {
            testoMessaggioPersonalizzato.text = 
                $"Se riuscirai a sconfiggere i boss di queste 5 epoche potrai finalmente ritornare a casa!\n\nBuona fortuna, {nome.ToUpper()}!";
        }
    }

    public void ProssimaSlide()
    {
        if (indiceSlideAttuale < slides.Length - 1)
        {
            indiceSlideAttuale++;
            AggiornaVisualizzazioneSlide();
        }
        else
        {
            CompletaTutorial();
        }
    }

    public void SlidePrecedente()
    {
        if (indiceSlideAttuale > 0)
        {
            indiceSlideAttuale--;
            AggiornaVisualizzazioneSlide();
        }
    }

    public void CompletaTutorial()
    {
        PlayerPrefs.SetInt("TutorialVisto", 1);
        PlayerPrefs.Save();

        gameObject.SetActive(false);

        if (pannelloLivelli != null)
        {
            pannelloLivelli.SetActive(true);
        }
    }

    private void AggiornaVisualizzazioneSlide()
    {
        AggiornaMessaggioPersonalizzato();

        for (int i = 0; i < slides.Length; i++)
        {
            if (slides[i] != null)
            {
                slides[i].SetActive(i == indiceSlideAttuale);
            }
        }

        if (bottonePrecedente != null)
        {
            bottonePrecedente.gameObject.SetActive(indiceSlideAttuale > 0);
        }

        if (bottoneSuccessivo != null)
        {
            // Se siamo all'ultima slide, nascondiamo 'Avanti' e mostriamo 'Inizia Gioco'
            bottoneSuccessivo.gameObject.SetActive(indiceSlideAttuale < slides.Length - 1);
        }

        if (bottoneIniziaGioco != null)
        {
            bottoneIniziaGioco.gameObject.SetActive(indiceSlideAttuale == slides.Length - 1);
        }
    }
}
