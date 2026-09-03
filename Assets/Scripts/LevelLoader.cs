using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void CaricaLivello(string nomeScena)
    {
        SceneManager.LoadScene(nomeScena);
    }

    // Aggiungi questa funzione per tornare al menu iniziale
    public void TornaAlMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}