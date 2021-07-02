using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public enum GameState
{
    GameOver, RoundOver, GamePlay, Wait
}
public class Gamemanager : MonoBehaviour
{
    float timer = 0f;
    float GameOverTimer = 150f;
    public GameObject timerText;

    public string[] uwu = new string[26];
    
    public static int enemyAmount = 0;
    public static int enemyKilled = 0;
    public GameState gameState;
    //singleton
    private static Gamemanager _instance;
    public static Gamemanager Instance { get { return _instance; } }
    private void Awake()
    {
        _instance = this;
    }

    public delegate void FNotify();
    public static event FNotify OnGameplay;
    public static event FNotify OnRoundOver;
    public static event FNotify OnGameOver;

    private void OnDisable()
    {
        PlayerController.OnPlayerDeath -= PlayerController_OnplayerDeath;
        PlayerController.bonuslink -= Bonus;

        enemy.OnEnemyDeath -=  enemy_OnEnemyDeath;
        Door.OnxD -= DankDoor;
    }

    private void OnEnable()
    {
        PlayerController.OnPlayerDeath += PlayerController_OnplayerDeath;
        PlayerController.bonuslink += Bonus;
        enemy.OnEnemyDeath += enemy_OnEnemyDeath;
        Door.OnxD += DankDoor;
    }
    string DankDoor()
    {
        string omg;
        int ran = Random.Range(0, 25);
            omg = uwu[ran];
        return omg;
    }
    void Bonus()
    {
        Update65(GameState.RoundOver);
    }
    private void PlayerController_OnplayerDeath()
    {
        OnGameOver.Invoke();
    }

    private void enemy_OnEnemyDeath()
    {
        enemyKilled++;
    }

    private void Update()
    {
        Debug.Log(timer);
        if (gameState == GameState.Wait)
        {
            timer += Time.deltaTime;
        }
        if (timer >= 2f)
        {
            Debug.Log("Start!");
            gameState = GameState.GamePlay;
            OnGameplay.Invoke();
            timer = 0;
        }
        if (gameState == GameState.GamePlay)
        {
            GameOverTimer -= Time.deltaTime;
            timerText.GetComponent<Text>().text = GameOverTimer.ToString("0");
            if (GameOverTimer <= 0)
            {
                gameState = GameState.GameOver;
                OnGameOver.Invoke();
                Debug.Log("Game Over");
            }
        }
        if (gameState == GameState.GamePlay)
        {
            if (enemyKilled >= enemyAmount)
            {
                gameState = GameState.RoundOver;
                OnRoundOver.Invoke();
                Debug.Log("You Win!");
            }
        }
    }
    private void Start()
    {
        gameState = GameState.Wait;
        timerText.GetComponent<Text>().text = GameOverTimer.ToString("0");
    }

    private void Update65(GameState currentState)
    {
        gameState = currentState;

        switch (currentState)
        {
            case GameState.Wait:
                break;
            case GameState.GamePlay:
                break;
            case GameState.GameOver:
                break;
            case GameState.RoundOver:
                break;
        }
    }
}
