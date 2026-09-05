using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour, IDamageable
{
    [Header("Parametri Vitali")]
    public float vitaMassima = 100f;
    public float vitaAttuale;

    [Header("Interfaccia Grafica")]
    public Slider sliderVita;

    private PlayerDefense difesa;
    
    // Evento per comunicare la morte
    public event Action<GameObject> OnDeath;
    
    private bool isMorto = false;

    void Start()
    {
        difesa = GetComponent<PlayerDefense>();
        vitaAttuale = vitaMassima;
        AggiornaBarraVita();
    }

    public void PrendiDanno(float quantitaDanno) 
    {
        if (isMorto) return;

        if (difesa != null && difesa.isInDifesa) {
            quantitaDanno = quantitaDanno * 0.2f;
        }

        vitaAttuale -= quantitaDanno;
        vitaAttuale = Mathf.Clamp(vitaAttuale, 0, vitaMassima);

        AggiornaBarraVita();

        if (vitaAttuale <= 0) {
            Muori();
        }
    }

    private void AggiornaBarraVita() 
    {
        if (sliderVita != null) 
        {
            sliderVita.maxValue = vitaMassima;
            sliderVita.value = vitaAttuale;

            Image fillImage = sliderVita.fillRect.GetComponent<Image>();
            if (fillImage != null)
            {
                float percentuale = vitaAttuale / vitaMassima;
                if (percentuale > 0.5f)
                    fillImage.color = Color.green;
                else if (percentuale > 0.25f)
                    fillImage.color = new Color(1f, 0.5f, 0f);
                else
                    fillImage.color = Color.red;
            }
        }
    }

    private void Muori() 
    {
        isMorto = true;
        
        // Notifichiamo gli ascoltatori (es. GameManager)
        OnDeath?.Invoke(gameObject);
        
        gameObject.SetActive(false);
    }
}
