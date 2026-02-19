using UnityEngine;

public class StoveCounterAnimation : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject particlesGameObject;
    

    private void Start(){
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;   
    }
    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e){
        bool showVisual=e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried|| e.state == StoveCounter.State.Burned;
        Show_Hide(showVisual);
    }
    private void Show_Hide(bool input){
      particlesGameObject.SetActive(input);
      stoveOnGameObject.SetActive(input);
    }

}
