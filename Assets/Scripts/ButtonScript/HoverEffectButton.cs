using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverEffectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 scalaOriginale;
    private bool inizializzato = false;

    [Header("Scala Ingrandimento")]
    public float moltiplicatore = 1.1f;

    void Awake() 
    {
        if (!inizializzato)
        {
            scalaOriginale = transform.localScale;
            inizializzato = true;
        }
    }

    void Start() 
    {
        // Se il bottone ha parti trasparenti grandi, abbassiamo la soglia a 0.1f per rendere il click molto più facile
        var img = GetComponent<UnityEngine.UI.Image>();
        if (img != null && img.sprite != null) 
        {
            try { img.alphaHitTestMinimumThreshold = 0.1f; } catch {}
        }
    }

    void OnEnable()
    {
        if (inizializzato)
        {
            // Ripristina la scala normale quando il pannello viene riattivato
            transform.localScale = scalaOriginale;
        }
    }

    void OnDisable() 
    {
        if (inizializzato)
        {
            // Assicurati che non rimanga ingrandito se il pannello viene chiuso mentre il mouse è sopra
            transform.localScale = scalaOriginale;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) 
    {
        transform.localScale = scalaOriginale * moltiplicatore;
    }

    public void OnPointerExit(PointerEventData eventData) 
    {
        transform.localScale = scalaOriginale;
    }
}
