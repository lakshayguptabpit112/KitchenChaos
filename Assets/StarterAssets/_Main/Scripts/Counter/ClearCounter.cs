using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact(Player player){
        if(!HasKitchenObject()){
            if(player.HasKitchenObject()){
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else{
                //DO Nothing
            }
        }
        else{
            if(player.HasKitchenObject()){
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)){
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectsSO())){
                        Debug.Log(GetKitchenObject());
                        GetKitchenObject().DestroySelf();
                    }
                }
                else{
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject)){
                        if(plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectsSO())){
                            Debug.Log(player.GetKitchenObject());
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                    else {
                        SwapObjects(player); // Swap the Kitchen Objects
                    }
                }
            } 
            else{
                GetKitchenObject().SetKitchenObjectParent(player);       
            }   
        }
    }
}