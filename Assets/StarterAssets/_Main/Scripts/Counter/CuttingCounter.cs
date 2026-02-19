using System;   
using UnityEngine;

public class CuttingCounter : BaseCounter, I_HasProgress
{
    [SerializeField] private CuttingRecipieSO[] cuttingRecipieSOArray;
    public event EventHandler<I_HasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public static EventHandler OnAnyCut;
    new public static void ResetStaticData(){
        OnAnyCut=null;
    }
    public EventHandler OnCut;

    private int cuttingProgress;
    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                if (HasRecipieWithInput(player.GetKitchenObject().GetKitchenObjectsSO())) {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;
                    CuttingRecipieSO cuttingRecipieSO = GetCuttingRecipieSOWithInput(GetKitchenObject().GetKitchenObjectsSO());
                    OnProgressChanged?.Invoke(this, new I_HasProgress.OnProgressChangedEventArgs {
                        progressNormalized = (float)cuttingProgress / cuttingRecipieSO.cuttingProgressMax
                    });
                }
            }
            else {
                //DO Nothing
            }
        } 
        else { // Counter has kitchen object
            if (player.HasKitchenObject()) {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectsSO())) {
                        OnProgressChanged?.Invoke(this, new I_HasProgress.OnProgressChangedEventArgs { progressNormalized = 0f }); //Not being Used as ( theres no recepie for uncut object on plate ) but good to have

                        Debug.Log("Pickup");
                        GetKitchenObject().DestroySelf();
                    }
                } 
                else {
                    // Added By Me: 
                    if (HasRecipieWithInput(player.GetKitchenObject().GetKitchenObjectsSO())) {
                    SwapObjects(player); // Swap the Kitchen Objects between the Player and the Cutting Counter
                    OnProgressChanged?.Invoke(this, new I_HasProgress.OnProgressChangedEventArgs { progressNormalized = 0f }); // progress reset on swap
                    cuttingProgress = 0;
                    }
                }
            } 
            else {
                OnProgressChanged?.Invoke(this, new I_HasProgress.OnProgressChangedEventArgs { progressNormalized = 0f }); // progress reset on pickup

                GetKitchenObject().SetKitchenObjectParent(player);       
            }   
        }
    }

    public override void InteractAlternate(Player player){
        if(HasKitchenObject() && !player.HasKitchenObject() && HasRecipieWithInput(GetKitchenObject().GetKitchenObjectsSO())){
            cuttingProgress++;
            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);
            CuttingRecipieSO cuttingRecipieSO = GetCuttingRecipieSOWithInput(GetKitchenObject().GetKitchenObjectsSO());
            OnProgressChanged?.Invoke(this ,new I_HasProgress.OnProgressChangedEventArgs{
                progressNormalized = (float) cuttingProgress / cuttingRecipieSO.cuttingProgressMax
            });
            if (cuttingProgress >= cuttingRecipieSO.cuttingProgressMax){
                KitchenObjectsSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectsSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO,this);
            }
        }
    }

    private KitchenObjectsSO GetOutputForInput(KitchenObjectsSO inputKitchenObjectSO){
        CuttingRecipieSO cuttingRecipieSO = GetCuttingRecipieSOWithInput(inputKitchenObjectSO);
        if(cuttingRecipieSO!= null){
            return cuttingRecipieSO.output;
        }
        else{
            return null;
        }
    }

    public bool HasRecipieWithInput(KitchenObjectsSO inputKitchenObjectSO){
        CuttingRecipieSO cuttingRecipieSO = GetCuttingRecipieSOWithInput(inputKitchenObjectSO);
        return cuttingRecipieSO!= null;
    }

    private CuttingRecipieSO GetCuttingRecipieSOWithInput(KitchenObjectsSO inputKitchenObjectSO){
        foreach(CuttingRecipieSO cuttingRecipieSO in cuttingRecipieSOArray){
            if (cuttingRecipieSO.input == inputKitchenObjectSO){
                return cuttingRecipieSO;
            }
        }
        return null;
    }
}