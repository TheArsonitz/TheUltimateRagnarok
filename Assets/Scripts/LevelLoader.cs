using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void CaricaLivello(string nomeScena)
    {
        SceneManager.LoadScene(nomeScena);
    }
}