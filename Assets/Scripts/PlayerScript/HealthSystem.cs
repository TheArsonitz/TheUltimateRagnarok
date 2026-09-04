using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{

    [Header("Parametri Vitali")]
    public float vitaMassima = 100f;
    public float vitaAttuale;

    [Header("Interfaccia Grafica")]
    public Image barraVita;

    private PlayerDefense difesa;
    
    void Start()
    {
        difesa = GetComponent<PlayerDefense>();
        vitaAttuale = vitaMassima;
        AggiornaBarraVita();
    }


    public void PrendiDanno(float quantitaDanno) {

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

    private void AggiornaBarraVita() {
        if(barraVita != null) {

            float percentuale = vitaAttuale / vitaMassima;
            barraVita.fillAmount = percentuale;

            if (percentuale > 0.5f)
                barraVita.color = Color.green;
            else if (percentuale > 0.25f)
                barraVita.color = new Color(1f, 0.5f, 0f);
            else
                barraVita.color = Color.red;

        }
    }

    private void Muori() {
        if (GameManager.instance != null) {
            GameManager.instance.GiocatoreMorto(gameObject.name);
        }
        gameObject.SetActive(false);
    }

}
