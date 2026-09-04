using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FixMapButtonRuntime : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        Image img = GetComponent<Image>();
        if (img != null)
        {
            img.alphaHitTestMinimumThreshold = 0.1f;
        }
    }

    void OnEnable()
    {
        if (anim != null)
        {
            anim.Play("Bottone_Normale", 0, 0f);
        }
    }
    
    void OnDisable()
    {
        if (anim != null)
        {
            anim.Play("Bottone_Normale", 0, 0f);
        }
    }
}
