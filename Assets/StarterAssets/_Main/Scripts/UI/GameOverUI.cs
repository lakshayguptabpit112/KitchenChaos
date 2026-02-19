using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    private int recipesDelivered;
    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChange;
        Hide();
    }
    private void GameManager_OnStateChange(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGamOver())
        {
            Show();
            SetrecipesDeliveredText();
        }
        else
        {
            Hide();
        }
    }
    private void SetrecipesDeliveredText()
    {
        recipesDeliveredText.text = DeliveryManager.Instance.GetRecipesDeliveredSuccessFully().ToString();
    }
    private void Show() {
        gameObject.SetActive(true);
    }
    private void Hide() {
        gameObject.SetActive(false);
    }

}
