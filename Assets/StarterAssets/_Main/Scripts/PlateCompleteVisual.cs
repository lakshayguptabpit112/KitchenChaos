using System;
using UnityEngine;
using System.Collections.Generic;

public class PlateCompleteVisualScript : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectsSO_GameObject{
        public KitchenObjectsSO kitchenObjectSO;
        public GameObject gameObject;  
    }
    [SerializeField ]private PlateKitchenObject plateKitchenObject;
    [SerializeField ]private List<KitchenObjectsSO_GameObject> kitchenObjectsSOGameObjectList;
    private void Start(){
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
        foreach (KitchenObjectsSO_GameObject kitchenObjectSOGameObject in kitchenObjectsSOGameObjectList){
            kitchenObjectSOGameObject.gameObject.SetActive(false);
        }
    }
    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e){
        foreach (KitchenObjectsSO_GameObject kitchenObjectSOGameObject in kitchenObjectsSOGameObjectList){
            if(kitchenObjectSOGameObject.kitchenObjectSO == e.kitchenObjectSO){
                kitchenObjectSOGameObject.gameObject.SetActive(true);
            }
        }
    }
}
