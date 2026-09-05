using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverEffectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 scalaOriginale;
    private bool inizializzato = false;

    [Header("Impostazioni Ingrandimento")]
    [Tooltip("Lascia vuoto per ingrandire questo stesso oggetto, oppure trascina l'oggetto grafico da ingrandire.")]
    public Transform bersaglioGrafico;
    public float moltiplicatore = 1.1f;

    private Transform TargetTransform 
    {
        get { return bersaglioGrafico != null ? bersaglioGrafico : transform; }
    }

    void Awake() 
    {
        if (!inizializzato)
        {
            scalaOriginale = TargetTransform.localScale;
            inizializzato = true;
        }
    }

    void Start() 
    {
        var img = GetComponent<UnityEngine.UI.Image>();
        if (img != null && img.sprite != null) 
        {
            try { img.alphaHitTestMinimumThreshold = 0f; } catch {} // Usiamo 0 per evitare bug
        }
    }

    void OnEnable()
    {
        if (inizializzato)
        {
            TargetTransform.localScale = scalaOriginale;
        }
    }

    void OnDisable() 
    {
        if (inizializzato)
        {
            TargetTransform.localScale = scalaOriginale;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) 
    {
        TargetTransform.localScale = scalaOriginale * moltiplicatore;
    }

    public void OnPointerExit(PointerEventData eventData) 
    {
        TargetTransform.localScale = scalaOriginale;
    }
}
