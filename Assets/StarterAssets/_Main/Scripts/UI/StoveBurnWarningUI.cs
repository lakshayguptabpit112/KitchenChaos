using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter; 
    private void Start(){
        stoveCounter.OnProgressChanged+=stoveCounte_OnProgressChanged;
        Hide ();
    }
    private void stoveCounte_OnProgressChanged(object sender , I_HasProgress.OnProgressChangedEventArgs e){
        float burnShowProgressAmount = 0.5f;
        bool show = stoveCounter.isFried() && e.progressNormalized >= burnShowProgressAmount;
        if(show){
            Show ();
        }
        else{
            Hide();
        }
    }

    private void Show (){
      gameObject.SetActive(true);
    }
    private void Hide (){
      gameObject.SetActive(false);  
    }

}
