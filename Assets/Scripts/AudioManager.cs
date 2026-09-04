using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;
    public AudioClip musicaSottofondo;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (audioSource == null) audioSource = GetComponent<AudioSource>();
            if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

            audioSource.loop = true;
            audioSource.playOnAwake = true;

            if (musicaSottofondo != null && audioSource.clip == null)
            {
                audioSource.clip = musicaSottofondo;
            }

            // Applica volume salvato
            float vol = PlayerPrefs.GetFloat("VolumeMaster", PlayerPrefs.GetFloat("VolumeGioco", 1f));
            audioSource.volume = vol;

            if (audioSource.clip != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ImpostaVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }
}
