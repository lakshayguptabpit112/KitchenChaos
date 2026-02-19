using System;
using UnityEngine;

public class Player : MonoBehaviour,I_KitchenObjectParent
{
    public static Player Instance{ get; private set; }
    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public BaseCounter selectedCounter;
    }
    [SerializeField] private Transform kitchenObjectHoldPoint;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;
    private KitchenObject kitchenObject;
    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private void Awake(){
        if(Instance != null ){
            Debug.LogError("MultiplayerNoooo");
        }
        Instance = this;
    }
    private void Start(){
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;  
    }
    private void GameInput_OnInteractAction(object sender, System.EventArgs e){
        if(!GameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null){
            selectedCounter.Interact(this); 
        }
    }
    private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e){
        if(!GameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null){
            selectedCounter.InteractAlternate(this);
        }
    }
    private void Update()
    {
       HandleMovement();
       HandleInteractions();
    }
    public bool IsWalking(){
        return isWalking;
    }
    private void HandleInteractions(){
        Vector2 inputVector = gameInput.GetNormalizedMovementComponent();
        Vector3 moveDir = new Vector3(inputVector.x,0f,inputVector.y);
        if(moveDir!= Vector3.zero){
            lastInteractDir = moveDir;
        }
        float intractDistance = 2f;
        if(Physics.Raycast(transform.position,lastInteractDir,out RaycastHit raycastHit,intractDistance,counterLayerMask)){
            if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)){
                if (selectedCounter != baseCounter){
                    SetSelectedChanged(baseCounter);
                }
            }
            else {
                SetSelectedChanged(null);
            }
        }
        else{
            SetSelectedChanged(null);
        }
    }
    private void HandleMovement(){
        Vector2 inputVector = gameInput.GetNormalizedMovementComponent();
        Vector3 moveDir = new Vector3(inputVector.x,0f,inputVector.y);
        float moveDistance = Time.deltaTime * moveSpeed;
        float playerRadius = .8f;
        float playerHight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHight,playerRadius,moveDir,moveDistance);
        if (!canMove){
           Vector3 moveDirx = new Vector3(moveDir.x,0,0).normalized;
            canMove = moveDir.x !=0 && !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHight,playerRadius,moveDirx,moveDistance);
            if (canMove) {
                moveDir = moveDirx;
            }
            else{
                Vector3 moveDirz = new Vector3(0,0,moveDir.z).normalized;
                canMove = moveDir.z !=0 && !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHight,playerRadius,moveDirz,moveDistance);
                if (canMove) {
                    moveDir = moveDirz;
                }
                else{
                    Debug.Log("CantWalk");
                }
            }  
        }
        if (canMove) {
        transform.position += moveDir * moveDistance;
        }
        isWalking = moveDir != Vector3.zero;
        float rotationSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir,Time.deltaTime * rotationSpeed);
    }
    private void SetSelectedChanged(BaseCounter selectedCounter){
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
                       selectedCounter = selectedCounter
                    });
    }
    public Transform GetKitchenObjectFollowTransform(){ 
        return kitchenObjectHoldPoint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject){
        this.kitchenObject = kitchenObject;
        if(kitchenObject!=null){
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
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
