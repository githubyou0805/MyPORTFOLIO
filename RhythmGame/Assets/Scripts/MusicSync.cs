using UnityEngine;

public class MusicSync : MonoBehaviour
{
    public AudioSource audioSource;
    public double startDspTime;

    void Start()
    {
        startDspTime = AudioSettings.dspTime + 1.0;
        audioSource.PlayScheduled(startDspTime);
    }

    public double MusicTime => AudioSettings.dspTime - startDspTime;
}
