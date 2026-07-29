using UnityEngine;
using UnityEngine.UI;

public class TrasparenzaMouse : MonoBehaviour
{
    void Start()
    {
        // Questo comando dice a Unity di ignorare i pixel trasparenti
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }
}