using UnityEngine;
using UnityEngine.UI;

public class ClassificaUI : MonoBehaviour
{
    [Header("Impostazioni Livello")]
    public int numeroLivello = 1;

    [Header("Testi a Schermo")]
    public Text testoTitoloLivello;
    public Text testoRecord1;
    public Text testoRecord2;
    public Text testoRecord3;

    [Header("Pulsanti Navigazione")]
    public Button bottonePrecedente;
    public Button bottoneSuccessivo;

    void OnEnable()
    {
        numeroLivello = 1;
        AggiornaTestiClassifica();
    }

    public void AggiornaTestiClassifica()
    {
        if (testoTitoloLivello != null)
            testoTitoloLivello.text = "LIVELLO " + numeroLivello;

        var records = ClassificaManager.OttieniClassifica(numeroLivello);

        if (testoRecord1 != null) testoRecord1.text = FormattaRigaRecord(1, records[0]);
        if (testoRecord2 != null) testoRecord2.text = FormattaRigaRecord(2, records[1]);
        if (testoRecord3 != null) testoRecord3.text = FormattaRigaRecord(3, records[2]);

        if (bottonePrecedente != null)
            bottonePrecedente.interactable = (numeroLivello > 1);
        if (bottoneSuccessivo != null)
            bottoneSuccessivo.interactable = (numeroLivello < 5);
    }

    private string FormattaRigaRecord(int posto, ClassificaManager.RecordClassifica record)
    {
        if (record.tempo >= 9999f || record.tempo <= 0f)
        {
            return $"{posto}° POSTO:  ---";
        }

        string nome = string.IsNullOrEmpty(record.nome) ? "EROE" : record.nome.ToUpper();
        return $"{posto}° POSTO:  {nome}  -  {FormattaTempo(record.tempo)}";
    }

    public void LivelloSuccessivo()
    {
        if (numeroLivello < 5)
        {
            numeroLivello++;
            AggiornaTestiClassifica();
        }
    }

    public void LivelloPrecedente()
    {
        if (numeroLivello > 1)
        {
            numeroLivello--;
            AggiornaTestiClassifica();
        }
    }

    public void ApriClassifica()
    {
        gameObject.SetActive(true);
    }

    public void ChiudiClassifica()
    {
        gameObject.SetActive(false);
    }

    private string FormattaTempo(float secondiAttuali)
    {
        if (secondiAttuali >= 9999f || secondiAttuali <= 0f) return "--:--";

        float minuti = Mathf.FloorToInt(secondiAttuali / 60);
        float secondi = Mathf.FloorToInt(secondiAttuali % 60);

        return string.Format("{0:00}:{1:00}", minuti, secondi);
    }
}