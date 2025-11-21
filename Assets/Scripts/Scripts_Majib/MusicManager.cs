using UnityEngine;
using System.Collections;
using System.Threading;

public class MusicManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource introSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip combatIntro;
    [SerializeField] private AudioClip combatMusic;

    [Header("Settings")]
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float introVolume = 1f;
    [SerializeField] private float musicVolume = 1f;

    private void Start()
    {
        musicSource.loop = true;
        musicSource.volume = 0f;
        Thread.Sleep(1000);
        StartFight();
    }

    public void StartFight()
    {
        StartCoroutine(PlayCombatMusic());
    }

    private IEnumerator PlayCombatMusic()
    {
        musicSource.clip = combatMusic;
        musicSource.volume = 0.4f;
        musicSource.Play();

        introSource.clip = combatIntro;
        introSource.volume = introVolume;
        introSource.Play();

        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, musicVolume, elapsed / fadeInDuration);
            yield return null;
        }
        
        musicSource.volume = musicVolume;
    }

}