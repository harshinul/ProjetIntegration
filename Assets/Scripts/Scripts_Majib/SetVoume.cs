using UnityEngine;

public class SetVoume : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetFloat("MasterVol", 100f);
        PlayerPrefs.SetFloat("MusicVol", 100f);
        PlayerPrefs.SetFloat("SFXVol", 100f);
    }

}
