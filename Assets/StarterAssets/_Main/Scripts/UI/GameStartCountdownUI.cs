using UnityEngine;
using TMPro;

public class GameStartCountdownUI : MonoBehaviour
{
    private Animator animator;
    private int previousCountdownNumber;
    private void Awake(){
        animator = GetComponent<Animator>();

    }
    [SerializeField] private TextMeshProUGUI countdownText;
    private void Start(){
        GameManager.Instance.OnStateChanged += GameManager_OnStateChange;
        Hide();
    }
    private void GameManager_OnStateChange( object sender , System.EventArgs e){
        if(GameManager.Instance.IsCountdownToStartActive()){
            Show();
        }
        else{
            Hide();
        }
    }
    private void Update(){
       int countdownNumber=Mathf.CeilToInt(GameManager.Instance.GetCountdownToStartTimer());
       countdownText.text= countdownNumber.ToString();
       if(previousCountdownNumber!=countdownNumber){
        previousCountdownNumber=countdownNumber;
        animator.SetTrigger("NoPopUp");
        SoundManager.Instance.PlayCountdownSound();
       }

    }
    private void Show(){
        gameObject.SetActive(true);
    }
    private void Hide(){
        gameObject.SetActive(false);
    }


}
