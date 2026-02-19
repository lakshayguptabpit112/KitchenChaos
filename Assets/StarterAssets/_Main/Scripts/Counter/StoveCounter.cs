using System;
using UnityEngine;

public class StoveCounter : BaseCounter,I_HasProgress
{
    public EventHandler <OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler<I_HasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public class OnStateChangedEventArgs: EventArgs{
        public State state;
    }
    public enum State{
        Idle,
        Frying,
        Fried,
        Burned,
    }
    [SerializeField] private FryingRecipieSO[] fryingRecipieSOArray;
    [SerializeField] private BurningRecipieSO[] burningRecipieSOArray;
    private float fryingTimer;
    private FryingRecipieSO fryingRecipieSO;
    private BurningRecipieSO burningRecipieSO;
    private State state;
    private float burningTimer;

    private void Start(){
       state = State.Idle;

    }

    private void Update(){
        if (HasKitchenObject()){
            switch (state){
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this ,new I_HasProgress.OnProgressChangedEventArgs{
                    progressNormalized = (float) fryingTimer / fryingRecipieSO.fryingTimerMax });
                    if(fryingTimer > fryingRecipieSO.fryingTimerMax){
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipieSO.output,this);
                        state = State.Fried;
                        burningTimer = 0f;
                        burningRecipieSO = GetBurningRecipieSOWithInput((GetKitchenObject().GetKitchenObjectsSO()));
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                            state = state
                        });
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this ,new I_HasProgress.OnProgressChangedEventArgs{
                    progressNormalized = (float) burningTimer / burningRecipieSO.BurningTimerMax });
                    if(burningTimer > burningRecipieSO.BurningTimerMax){
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipieSO.output,this);
                        state = State.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                            state = state
                        });                        
                    }
                    break;
                case State.Burned:
                    OnProgressChanged?.Invoke(this ,new I_HasProgress.OnProgressChangedEventArgs{
                    progressNormalized = 0f});
                    Debug.Log("Burned");
                    break;
            }


        }

    }
    public override void Interact(Player player){
        if(!HasKitchenObject()){
            if(player.HasKitchenObject()){
                if(HasRecipieWithInput(player.GetKitchenObject().GetKitchenObjectsSO())){
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipieSO = GetFryingRecipieSOWithInput((GetKitchenObject().GetKitchenObjectsSO()));
                    state = State.Frying;
                    fryingTimer = 0f;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                        state = state
                    });
                    OnProgressChanged?.Invoke(this ,new I_HasProgress.OnProgressChangedEventArgs{
                    progressNormalized = (float) fryingTimer / fryingRecipieSO.fryingTimerMax });
                }
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
                        state = State.Idle;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                            state = state
                        });
                        OnProgressChanged?.Invoke(this ,new I_HasProgress.OnProgressChangedEventArgs{
                            progressNormalized = 0f}); 
                    }
                }
                else {
                    // Added By Me: 
                    if (HasRecipieWithInput(player.GetKitchenObject().GetKitchenObjectsSO())) { // Check if the player's object has a valid frying recipe before swapping
                        SwapObjects(player); // Swap the Kitchen Objects between the Player and the Counter
                        fryingRecipieSO = GetFryingRecipieSOWithInput((GetKitchenObject().GetKitchenObjectsSO())); // Reset Stove Logic for the new kitchen object
                        state = State.Frying;
                        fryingTimer = 0f; 

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = state
                        });
                        OnProgressChanged?.Invoke(this, new I_HasProgress.OnProgressChangedEventArgs { // Reset progress for the new frying process
                            progressNormalized = (float)fryingTimer / fryingRecipieSO.fryingTimerMax
                        });
                    }
                }
            }
            else{
                GetKitchenObject().SetKitchenObjectParent(player); 
                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                        state = state
                });
                OnProgressChanged?.Invoke(this ,new I_HasProgress.OnProgressChangedEventArgs{
                progressNormalized = 0f}); 
            }   
        }
    }
    private KitchenObjectsSO GetOutputForInput(KitchenObjectsSO inputKitchenObjectSO){
        FryingRecipieSO fryingRecipieSO = GetFryingRecipieSOWithInput(inputKitchenObjectSO);
        if(fryingRecipieSO!= null){
            return fryingRecipieSO.output;
        }
        else{
            return null;
        }
    }
    public bool HasRecipieWithInput(KitchenObjectsSO inputKitchenObjectSO){
        FryingRecipieSO fryingRecipieSO = GetFryingRecipieSOWithInput(inputKitchenObjectSO);
        return fryingRecipieSO!= null;
    }
    private FryingRecipieSO GetFryingRecipieSOWithInput(KitchenObjectsSO inputKitchenObjectSO){
        foreach(FryingRecipieSO fryingRecipieSO in fryingRecipieSOArray){
            if (fryingRecipieSO.input == inputKitchenObjectSO){
                return fryingRecipieSO;
            }
        }
        return null;
    }
    private BurningRecipieSO GetBurningRecipieSOWithInput(KitchenObjectsSO inputKitchenObjectSO){
        foreach(BurningRecipieSO burningRecipieSO in burningRecipieSOArray){
            if (burningRecipieSO.input == inputKitchenObjectSO){
                return burningRecipieSO;
            }
        }
        return null;
    }
    public bool isFried(){
        return state == State.Fried;
    }
}
