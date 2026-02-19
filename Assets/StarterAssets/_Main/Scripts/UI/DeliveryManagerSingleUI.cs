using UnityEngine;
using TMPro;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeName;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;
        private void Awake(){
        iconTemplate.gameObject.SetActive(false);
    }
    public void SetRecipeName( RecipeSO recipeSO ){
        recipeName.text = recipeSO.recipeName;
        foreach (Transform child in iconContainer){
            if(child == iconTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach(KitchenObjectsSO kitchenObjectSO in recipeSO.kitchenObjectSOList){
            Transform iconTransform =Instantiate(iconTemplate,iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<UnityEngine.UI.Image>().sprite = kitchenObjectSO.sprite;
        }
    }
}
