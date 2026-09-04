using UnityEngine;

public class SelezioneLivelli : MonoBehaviour
{
    [Header("Elementi da scorrere")]
    public GameObject[] livelli;
    public GameObject[] personaggi;

    private int indiceAttuale = 0;

    void Start()
    {
        AggiornaVisualizzazione();
    }

    public void LivelloSuccessivo()
    {
        if (indiceAttuale < livelli.Length - 1)
        {
            indiceAttuale++;
            AggiornaVisualizzazione();
        }
    }

    public void LivelloPrecedente()
    {
        if (indiceAttuale > 0)
        {
            indiceAttuale--;
            AggiornaVisualizzazione();
        }
    }

    private void AggiornaVisualizzazione()
    {
        for (int i = 0; i < livelli.Length; i++)
        {
            livelli[i].SetActive(false);
            personaggi[i].SetActive(false);
        }

        livelli[indiceAttuale].SetActive(true);
        personaggi[indiceAttuale].SetActive(true);
    }
}