using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;
    [SerializeField] private GameObject[] spriteGameObject;

    [SerializeField] private KitchenObjectsSO kitchenObjectSO;
    public override void Interact(Player player){
        if (!player.HasKitchenObject()){
            KitchenObject.SpawnKitchenObject(kitchenObjectSO,player);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
    
}
