using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {get;private set;}
    [SerializeField ]private AudioClipsRefSO audioClipsRefSO;
    private float volume=1f;
    private void Awake(){
        Instance = this;
        volume=PlayerPrefs.GetFloat("SoundEffectsVolume",1f);


    }
    private void Start(){
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;

    }
    private void CuttingCounter_OnAnyCut( object sender , System.EventArgs e){
        CuttingCounter cuttingCounter= sender as CuttingCounter;
        PlaySound(audioClipsRefSO.chop,cuttingCounter.transform.position);
    }
    private void DeliveryManager_OnRecipeSuccess( object sender , System.EventArgs e){
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipsRefSO.deliverySuccess,deliveryCounter.transform.position);
    }
    private void DeliveryManager_OnRecipeFailed( object sender , System.EventArgs e){
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipsRefSO.deliveryFail,deliveryCounter.transform.position);
    }
    private void Player_OnPickedSomething( object sender , System.EventArgs e){
        PlaySound(audioClipsRefSO.objectPickup,Player.Instance.transform.position);
    }
    private void BaseCounter_OnAnyObjectPlacedHere( object sender , System.EventArgs e){
        BaseCounter baseCounter= sender as BaseCounter;
        PlaySound(audioClipsRefSO.objectDrop,baseCounter.transform.position);
    }
    private void TrashCounter_OnAnyObjectTrashed( object sender , System.EventArgs e){
        TrashCounter trashCounter= sender as TrashCounter;
        PlaySound(audioClipsRefSO.trash,trashCounter.transform.position);
    }
    public void PlayFootStepsSound(Vector3 position,float volume=1f){ 
        PlaySound(audioClipsRefSO.footstep,position,volume);
    }
    public void PlayCountdownSound(){ 
        PlaySound(audioClipsRefSO.warning,Vector3.zero);
    }
    public void PlayWarningSound(Vector3 position){ 
        PlaySound(audioClipsRefSO.warning,position);
    }



        
    private void PlaySound(AudioClip[] audioClipArray,Vector3 position,float volume=1f){
        PlaySound(audioClipArray[Random.Range(0,audioClipArray.Length)],position,volume);
    }
    private void PlaySound(AudioClip audioClip,Vector3 position,float volumeMultiplyer=1f){
        AudioSource.PlayClipAtPoint(audioClip,position,volumeMultiplyer * volume);
    }
    public void ChangeVolume(){
        volume+=.1f;
        if (volume >1.5f){
            volume=0f;
        }
        PlayerPrefs.SetFloat("SoundEffectsVolume",volume);
        PlayerPrefs.Save();

    }
    public float GetVolume(){
        return volume;
    }

}
