using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image timeImage;
    private void Start()

    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChange;
        Hide();
        timeImage.fillAmount = 0f;
    }
    private void Update() {
        timeImage.fillAmount = GameManager.Instance.GetGamePlayingTimerNormalized();
    }
    private void GameManager_OnStateChange(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    private void Show() {
        gameObject.SetActive(true);
    }
    private void Hide() {
        gameObject.SetActive(false);
    }

}
