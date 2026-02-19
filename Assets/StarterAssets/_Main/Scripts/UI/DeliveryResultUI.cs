using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Image backgroundImage; 
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failedColor;
    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failedSprite;
    private Animator animator;
    private void Awake(){
        animator = GetComponent<Animator>();
    }
   private void Start(){
    DeliveryManager.Instance.OnRecipeSuccess+=DeliveryManager_OnRecipeSuccess;
    DeliveryManager.Instance.OnRecipeFailed+=DeliveryManager_OnRecipeFailed;
    Hide ();
   }
   private void DeliveryManager_OnRecipeSuccess(object sender , System.EventArgs e){
    Show ();
    animator.SetTrigger("PopUp");
    backgroundImage.color = successColor;
    iconImage.sprite = successSprite;
    messageText.text = "Delivery\nSuccess";
    }
    private void DeliveryManager_OnRecipeFailed(object sender , System.EventArgs e){
        Show ();
        animator.SetTrigger("PopUp");
        backgroundImage.color = failedColor;
        iconImage.sprite = failedSprite;
        messageText.text = "Delivery\nFailed";
    }
    private void Show (){
      gameObject.SetActive(true);
    }
    private void Hide (){
      gameObject.SetActive(false);  
    }

}
