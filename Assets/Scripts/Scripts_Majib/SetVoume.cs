using UnityEngine;

public class SetVoume : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPrefs.SetFloat("MasterVol", 100f);
        PlayerPrefs.SetFloat("MusicVol", 100f);
        PlayerPrefs.SetFloat("SFXVol", 100f);
    }

}
