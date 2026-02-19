using UnityEngine;

public class StoveBurnFlashingUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter; 
    private Animator animator;
    private void Awake(){
        animator = GetComponent<Animator>();
    }
    private void Start(){
        stoveCounter.OnProgressChanged+=stoveCounter_OnProgressChanged;
        animator.SetBool("IsFlashing",false);
    }
    private void stoveCounter_OnProgressChanged(object sender , I_HasProgress.OnProgressChangedEventArgs e){
        float burnShowProgressAmount = 0.5f;
        bool show = stoveCounter.isFried() && e.progressNormalized >= burnShowProgressAmount;
        animator.SetBool("IsFlashing",show);
    }
}
