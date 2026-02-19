using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool lookAtInverted;
    private void LateUpdate()
    {
        if(lookAtInverted){
            Vector3 dirFromCamera =transform.position - Camera.main.transform.position;
            transform.LookAt(transform.position + dirFromCamera);
        }
        else{
        transform.LookAt(Camera.main.transform);
        }
    }
}
