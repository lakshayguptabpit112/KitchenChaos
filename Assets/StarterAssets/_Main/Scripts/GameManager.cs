using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }
    private State state;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 180f;
    private bool isGamePaused = false;

    private void Awake()
    {
        state = State.WaitingToStart;
        Instance = this;
    }
    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;


    }
    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                break;
            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f)
                {
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                Debug.Log("GameEnd");
                break;
        }

    }
    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        if (isGamePaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();

        }

    }
    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (state == State.WaitingToStart){
            state = State.CountdownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
            
        }
    }
    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;

    }
    public bool IsCountdownToStartActive()
    {
        return state == State.CountdownToStart;

    }
    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;

    }

    public float GetGamePlayingTimerNormalized()
    {
        return (1 - (gamePlayingTimer / gamePlayingTimerMax));

    }
    public bool IsGamOver()
    {
        return state == State.GameOver;
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        isGamePaused = !isGamePaused;
        OnGamePaused?.Invoke(this, EventArgs.Empty);

    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isGamePaused = !isGamePaused;
        OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        
    }
}