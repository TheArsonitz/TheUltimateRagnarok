using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverEffectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 scalaOriginale;

    [Header("Scala Ingrandimento")]
    public float moltiplicatore = 1.1f;


    void Start() {
        scalaOriginale = transform.localScale;

        GetComponent<UnityEngine.UI.Image>().alphaHitTestMinimumThreshold = 0.5f;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        transform.localScale = scalaOriginale * moltiplicatore;
    }

    public void OnPointerExit(PointerEventData eventData) {
        transform.localScale = scalaOriginale;
    }

}
