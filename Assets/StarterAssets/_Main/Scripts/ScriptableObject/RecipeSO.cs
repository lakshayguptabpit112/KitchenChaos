using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    public List<KitchenObjectsSO> kitchenObjectSOList;
    public string recipeName;
}

