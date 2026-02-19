using UnityEngine;
using System;

public class BaseCounter : MonoBehaviour , I_KitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlacedHere;
    public static void ResetStaticData(){
        OnAnyObjectPlacedHere=null;
    }
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    //Added By Me:
    
    protected void SwapObjects(Player player) { // Swap the Kitchen Objects between the Player and the Counter

        KitchenObjectsSO counterObjectSO = GetKitchenObject().GetKitchenObjectsSO(); // Save reference to what is currently on the counter
    
        KitchenObjectsSO playerObjectSO = player.GetKitchenObject().GetKitchenObjectsSO(); // Save reference to what is currently in the player's hand

        GetKitchenObject().DestroySelf();   // Remove the object from the counter

        player.GetKitchenObject().DestroySelf();  // Remove the object from the player's hand


        KitchenObject.SpawnKitchenObject(counterObjectSO, player); //spawn the object of player into the counter

        KitchenObject.SpawnKitchenObject(playerObjectSO, this);    //spawn the object of counter into the player's hand
    }
    //End Added By Me :)

    public virtual void Interact(Player player){
        Debug.LogError("BaseCounterInteracted();");
    }
    public virtual void InteractAlternate(Player player){
        Debug.Log("BaseCounterInteractedAlternate();");
    }
    
    public Transform GetKitchenObjectFollowTransform(){ 
        return counterTopPoint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject){
        this.kitchenObject = kitchenObject;
        if(kitchenObject!=null){
            OnAnyObjectPlacedHere?.Invoke(this,EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject(){
        return kitchenObject;
    }
    public void ClearKitchenObject(){
        kitchenObject = null;
    }
    public bool HasKitchenObject(){
        return kitchenObject != null;
    }
}
