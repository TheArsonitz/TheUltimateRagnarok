using UnityEngine;
using UnityEngine.UI;

public class PannelloComandiUI : MonoBehaviour
{
    [Header("Pulsanti Selezione Set")]
    public Button btnSetWASD;
    public Button btnSetTastierino;
    public Image imgBtnWASD;
    public Image imgBtnTastierino;

    [Header("Colonne Testo")]
    public Text testoTasti;
    public Text testoSpiegazioni;

    private readonly Color coloreAttivo = new Color(0.95f, 0.61f, 0.07f); // Oro / Ambra
    private readonly Color coloreInattivo = new Color(0.2f, 0.23f, 0.3f); // Grigio-blu scuro

    void Awake()
    {
        if (btnSetWASD != null) btnSetWASD.onClick.AddListener(() => SelezionaSet(0));
        if (btnSetTastierino != null) btnSetTastierino.onClick.AddListener(() => SelezionaSet(1));
    }

    void OnEnable()
    {
        AggiornaVisualizzazione();
    }

    public void ImpostaSetWASD()
    {
        SelezionaSet(0);
    }

    public void ImpostaSetTastierino()
    {
        SelezionaSet(1);
    }

    public void SelezionaSet(int indiceSet)
    {
        PlayerPrefs.SetInt("SetComandiP1", indiceSet);
        PlayerPrefs.Save();
        MenuController.AggiornaGiocatoriInScena();
        AggiornaVisualizzazione();
    }

    public void AggiornaVisualizzazione()
    {
        int setAttuale = PlayerPrefs.GetInt("SetComandiP1", 0);

        if (imgBtnWASD != null) imgBtnWASD.color = (setAttuale == 0) ? coloreAttivo : coloreInattivo;
        if (imgBtnTastierino != null) imgBtnTastierino.color = (setAttuale == 1) ? coloreAttivo : coloreInattivo;

        if (testoTasti != null)
        {
            if (setAttuale == 0)
            {
                testoTasti.text = "[ W ]\n\n" +
                                  "[ A ] / [ D ]\n\n" +
                                  "[ SPAZIO ]\n\n" +
                                  "[ S ]\n\n" +
                                  "[ E ]\n\n" +
                                  "[ ESC ]";
            }
            else
            {
                testoTasti.text = "[ 8 ]\n\n" +
                                  "[ 4 ] / [ 6 ]\n\n" +
                                  "[ 0 ]\n\n" +
                                  "[ 5 ]\n\n" +
                                  "[ 7 ]\n\n" +
                                  "[ ESC ]";
            }
        }

        if (testoSpiegazioni != null)
        {
            testoSpiegazioni.text = "Salto (Doppio salto consentito)\n\n" +
                                    "Movimento Sinistra o Destra\n\n" +
                                    "Attacco in mischia\n\n" +
                                    "Parata difensiva con scudo\n\n" +
                                    "Raccolta / Interazione oggetti\n\n" +
                                    "Menu di pausa";
        }
    }
}
