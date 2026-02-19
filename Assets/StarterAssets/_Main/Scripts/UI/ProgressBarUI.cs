using UnityEngine;
using UnityEngine.UI;


public class ProgressBarUI : MonoBehaviour
{
[SerializeField] private GameObject hasProgressGameObject;
[SerializeField] private Image barImage;
private I_HasProgress hasProgress;
private void Start(){
        hasProgress = hasProgressGameObject.GetComponent<I_HasProgress>();
        if (hasProgress == null){
          Debug.LogError("Video Dekh Tuni koi galti kari hei");
        }
        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
        barImage.fillAmount = 0f;
        Hide();
    }
    private void HasProgress_OnProgressChanged(object sender, I_HasProgress.OnProgressChangedEventArgs e){
        barImage.fillAmount = e.progressNormalized;  
        if(e.progressNormalized==0 || e.progressNormalized==1){
            Hide();            
        }
        else{
            Show(); 
        }  

    }
    private void Show (){
      gameObject.SetActive(true);
    }
    private void Hide (){
      gameObject.SetActive(false);  
    }

}
