using UnityEngine;

public class ClassificaManager : MonoBehaviour
{
    [System.Serializable]
    public struct RecordClassifica
    {
        public string nome;
        public float tempo;

        public RecordClassifica(string nome, float tempo)
        {
            this.nome = nome;
            this.tempo = tempo;
        }
    }

    public static void SalvaTempo(int livello, string nomeGiocatore, float nuovoTempo)
    {
        if (string.IsNullOrEmpty(nomeGiocatore)) nomeGiocatore = "Eroe";

        float tempo1 = PlayerPrefs.GetFloat("Livello" + livello + "_Record1_Tempo", PlayerPrefs.GetFloat("Livello" + livello + "_Record1", 9999f));
        string nome1 = PlayerPrefs.GetString("Livello" + livello + "_Record1_Nome", "");

        float tempo2 = PlayerPrefs.GetFloat("Livello" + livello + "_Record2_Tempo", PlayerPrefs.GetFloat("Livello" + livello + "_Record2", 9999f));
        string nome2 = PlayerPrefs.GetString("Livello" + livello + "_Record2_Nome", "");

        float tempo3 = PlayerPrefs.GetFloat("Livello" + livello + "_Record3_Tempo", PlayerPrefs.GetFloat("Livello" + livello + "_Record3", 9999f));
        string nome3 = PlayerPrefs.GetString("Livello" + livello + "_Record3_Nome", "");

        if (nuovoTempo < tempo1)
        {
            PlayerPrefs.SetFloat("Livello" + livello + "_Record3_Tempo", tempo2);
            PlayerPrefs.SetString("Livello" + livello + "_Record3_Nome", nome2);

            PlayerPrefs.SetFloat("Livello" + livello + "_Record2_Tempo", tempo1);
            PlayerPrefs.SetString("Livello" + livello + "_Record2_Nome", nome1);

            PlayerPrefs.SetFloat("Livello" + livello + "_Record1_Tempo", nuovoTempo);
            PlayerPrefs.SetString("Livello" + livello + "_Record1_Nome", nomeGiocatore);
        }
        else if (nuovoTempo < tempo2)
        {
            PlayerPrefs.SetFloat("Livello" + livello + "_Record3_Tempo", tempo2);
            PlayerPrefs.SetString("Livello" + livello + "_Record3_Nome", nome2);

            PlayerPrefs.SetFloat("Livello" + livello + "_Record2_Tempo", nuovoTempo);
            PlayerPrefs.SetString("Livello" + livello + "_Record2_Nome", nomeGiocatore);
        }
        else if (nuovoTempo < tempo3)
        {
            PlayerPrefs.SetFloat("Livello" + livello + "_Record3_Tempo", nuovoTempo);
            PlayerPrefs.SetString("Livello" + livello + "_Record3_Nome", nomeGiocatore);
        }

        PlayerPrefs.Save();
    }

    public static void SalvaTempo(int livello, float nuovoTempo)
    {
        string nomeDefault = PlayerPrefs.GetString("PlayerName", "Eroe");
        SalvaTempo(livello, nomeDefault, nuovoTempo);
    }

    public static RecordClassifica[] OttieniClassifica(int livello)
    {
        float t1 = PlayerPrefs.GetFloat("Livello" + livello + "_Record1_Tempo", PlayerPrefs.GetFloat("Livello" + livello + "_Record1", 0f));
        string n1 = PlayerPrefs.GetString("Livello" + livello + "_Record1_Nome", "");

        float t2 = PlayerPrefs.GetFloat("Livello" + livello + "_Record2_Tempo", PlayerPrefs.GetFloat("Livello" + livello + "_Record2", 0f));
        string n2 = PlayerPrefs.GetString("Livello" + livello + "_Record2_Nome", "");

        float t3 = PlayerPrefs.GetFloat("Livello" + livello + "_Record3_Tempo", PlayerPrefs.GetFloat("Livello" + livello + "_Record3", 0f));
        string n3 = PlayerPrefs.GetString("Livello" + livello + "_Record3_Nome", "");

        return new RecordClassifica[]
        {
            new RecordClassifica(n1, t1),
            new RecordClassifica(n2, t2),
            new RecordClassifica(n3, t3)
        };
    }
}