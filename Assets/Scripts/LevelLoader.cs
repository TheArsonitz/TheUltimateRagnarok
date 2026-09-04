using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;

    void Awake()
    {
        // Imposta l'istanza per la scena corrente
        instance = this;
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    public void CaricaLivello(string nomeScena)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nomeScena);
    }

    // Funzione per tornare al menu iniziale
    public void TornaAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // Metodo statico richiamabile direttamente da qualunque script
    public static void CaricaMenuPrincipale()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}