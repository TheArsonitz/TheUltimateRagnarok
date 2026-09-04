using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelezioneLivelli : MonoBehaviour
{
    [Header("Elementi Mappa Livelli")]
    public GameObject[] livelli;
    public GameObject[] personaggi;

    void Start()
    {
        AggiornaStatoLivelli();
    }

    void OnEnable()
    {
        AggiornaStatoLivelli();
    }

    public void AggiornaStatoLivelli()
    {
        // Il livello 1 e' sempre sbloccato di default
        int livelloMaxSbloccato = PlayerPrefs.GetInt("LivelloMaxSbloccato", 1);

        for (int i = 0; i < livelli.Length; i++)
        {
            if (livelli[i] == null) continue;

            int numeroLivello = i + 1;
            bool sbloccato = (numeroLivello <= livelloMaxSbloccato);

            // Manteniamo visibile il nodo del livello sulla mappa
            livelli[i].SetActive(true);

            // Gestione interazione del pulsante
            Button btn = livelli[i].GetComponent<Button>();
            if (btn != null)
            {
                btn.interactable = sbloccato;
            }

            // Aspetto visivo del pulsante livello
            Image img = livelli[i].GetComponent<Image>();
            if (img != null)
            {
                img.color = sbloccato ? Color.white : new Color(0.35f, 0.35f, 0.35f, 0.65f);
            }

            // Testo con il numero del livello
            Text txt = livelli[i].GetComponentInChildren<Text>();
            if (txt != null)
            {
                txt.color = sbloccato ? Color.white : new Color(0.6f, 0.6f, 0.6f, 0.65f);
            }

            // Disattiviamo l'ingrandimento in hover se bloccato
            EventTrigger trigger = livelli[i].GetComponent<EventTrigger>();
            if (trigger != null)
            {
                trigger.enabled = sbloccato;
            }

            // Aspetto visivo del personaggio associato
            if (personaggi != null && i < personaggi.Length && personaggi[i] != null)
            {
                personaggi[i].SetActive(true);
                Image imgPersonaggio = personaggi[i].GetComponent<Image>();
                if (imgPersonaggio != null)
                {
                    // Personaggio oscurato/silhouette se il livello e' bloccato
                    imgPersonaggio.color = sbloccato ? Color.white : new Color(0.2f, 0.2f, 0.2f, 0.45f);
                }
            }
        }
    }

    [ContextMenu("Reset Progresso")]
    public void ResetProgresso()
    {
        PlayerPrefs.SetInt("LivelloMaxSbloccato", 1);
        PlayerPrefs.Save();
        AggiornaStatoLivelli();
        Debug.Log("[SelezioneLivelli] Progresso resettato: solo Livello 1 sbloccato.");
    }

    [ContextMenu("Sblocca Tutti i Livelli")]
    public void SbloccaTuttiILivelli()
    {
        PlayerPrefs.SetInt("LivelloMaxSbloccato", 5);
        PlayerPrefs.Save();
        AggiornaStatoLivelli();
        Debug.Log("[SelezioneLivelli] Tutti i 5 livelli sono stati sbloccati!");
    }
}