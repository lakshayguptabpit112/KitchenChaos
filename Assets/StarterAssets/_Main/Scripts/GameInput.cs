using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance{ get; private set; }
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;

    public enum Binding{
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Interact,
        InteractAlternate,
        Pause,
    }
    private PlayerInputActions playerInput;
    private void Awake()
    {
        Instance = this;
        playerInput = new PlayerInputActions();
        if (PlayerPrefs.HasKey("InputBindings")){
            playerInput.LoadBindingOverridesFromJson(PlayerPrefs.GetString("InputBindings"));
        }
        playerInput.Player.Enable();
        playerInput.Player.Interact.performed += Interact_performed;
        playerInput.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInput.Player.Pause.performed += Pause_performed;
        
    }
    private void OnDestroy(){
        playerInput.Player.Disable();
        playerInput.Player.Interact.performed -= Interact_performed;
        playerInput.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInput.Player.Pause.performed -= Pause_performed;
        playerInput.Dispose();
        
    }
    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (OnInteractAction != null)
        {
            OnInteractAction(this, EventArgs.Empty);
        }

    }
    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj){
        if (OnInteractAlternateAction!=null){
            OnInteractAlternateAction(this,EventArgs.Empty);
        }
    }
    public Vector2 GetNormalizedMovementComponent(){
        Vector2 inputVector = playerInput.Player.Move.ReadValue<Vector2>();
        inputVector =  inputVector.normalized;
        return inputVector;
    }
    
    public string GetBindingText(Binding binding){
        switch (binding) {
            default:
            case Binding.MoveUp:
                return playerInput.Player.Move.bindings[1].ToDisplayString();
            case Binding.MoveDown:
                return playerInput.Player.Move.bindings[2].ToDisplayString();
            case Binding.MoveLeft:
                return playerInput.Player.Move.bindings[3].ToDisplayString();
            case Binding.MoveRight:
                return playerInput.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playerInput.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return playerInput.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInput.Player.Pause.bindings[0].ToDisplayString();

        }
    }
    public void RebindBinding(Binding binding, Action onActionRebound) {
        playerInput.Player.Disable();
        InputAction inputAction;
        int bindingIndex;
                switch (binding) {
            default:
            case Binding.MoveUp:
                inputAction=playerInput.Player.Move;
                bindingIndex=1;
                break;
            case Binding.MoveDown:
                inputAction=playerInput.Player.Move;
                bindingIndex=2;
                break;
            case Binding.MoveLeft:
                inputAction=playerInput.Player.Move;
                bindingIndex=3;
                break;
            case Binding.MoveRight:
                inputAction=playerInput.Player.Move;
                bindingIndex=4;
                break;
            case Binding.Interact:
                inputAction=playerInput.Player.Interact;
                bindingIndex=0;
                break;
            case Binding.InteractAlternate:
                inputAction=playerInput.Player.InteractAlternate;
                bindingIndex=0;
                break;
            case Binding.Pause:
                inputAction=playerInput.Player.Pause;
                bindingIndex=0;
                break;
        }
        inputAction.PerformInteractiveRebinding(bindingIndex)
        .OnComplete(callback => {
            callback.Dispose(); 
            playerInput.Player.Enable();
            onActionRebound();
            PlayerPrefs.SetString("InputBindings",playerInput.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
            OnBindingRebind?.Invoke(this,EventArgs.Empty);
            
        })
        .Start();
    }
}

