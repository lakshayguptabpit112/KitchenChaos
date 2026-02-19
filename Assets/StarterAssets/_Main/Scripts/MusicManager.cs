using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance{ get; private set; }
    private AudioSource audioSource;
    private float volume=.3f;
    private void Awake(){
        audioSource= GetComponent<AudioSource>();
        Instance=this;
        volume=PlayerPrefs.GetFloat("MusicVolume",.3f);
        audioSource.volume =volume;
    }
    public void ChangeVolume(){
        volume+=.1f;
        if (volume >1.5f){
            volume=0f;
        }
        audioSource.volume =volume;
        PlayerPrefs.SetFloat("MusicVolume",volume);
        PlayerPrefs.Save();

    }
    public float GetVolume(){
        return volume;
    }
}
