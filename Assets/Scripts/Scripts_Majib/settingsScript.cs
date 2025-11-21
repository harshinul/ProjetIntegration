using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class settingsScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TMP_Dropdown graphicsDropdown;
    public Slider masterVol,musicVol,sfxVol;
    public AudioMixer mainAudioMixer;
    public void ChangeQuality()
    {
        QualitySettings.SetQualityLevel(graphicsDropdown.value);
    }
    public void ChangeMasterVolume()
    {
        mainAudioMixer.SetFloat("MasterVol",masterVol.value);
        //PlayerPrefs.SetFloat("MasterVol", masterVol.value);
    }
    public void ChangeMusicVolume()
    {
        mainAudioMixer.SetFloat("MusicVol", musicVol.value);
        //PlayerPrefs.SetFloat("MusicVol", musicVol.value);
    }
    public void ChangeSfxVolume()
    {
        mainAudioMixer.SetFloat("SFXVol", sfxVol.value);
<<<<<<< HEAD
        //PlayerPrefs.SetFloat("SFXVol", sfxVol.value);
=======
>>>>>>> BrancheHarsh
    }
    void Start()
    {
        //masterVol.value = PlayerPrefs.GetFloat("MasterVol", 0f);
        //musicVol.value = PlayerPrefs.GetFloat("MusicVol", 0f);
        //sfxVol.value = PlayerPrefs.GetFloat("SFXVol", 0f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
