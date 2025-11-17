using System.Collections;
using UnityEngine;

public class SoundPlayerScript : MonoBehaviour
{
    bool SoundIsPlaying = false;

    public void PlaySound(AudioClip audio, Transform position, float volume, float duration = 0)
    {
        if (!SoundIsPlaying)
        {
            StopAllCoroutines();
            StartCoroutine(PlaySoundCouroutine(audio, transform, volume, duration));
        }
    }

    IEnumerator PlaySoundCouroutine(AudioClip audio, Transform position, float volume, float duration)
    {
        SoundIsPlaying = true;
        if (duration == 0f)
        {
            duration = audio.length;
        }
        SFXManager.Instance.PlaySFX(audio, position, volume);
        yield return new WaitForSeconds(duration);
        SoundIsPlaying = false;
    }
}
