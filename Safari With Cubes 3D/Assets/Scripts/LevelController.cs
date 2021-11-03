using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public static LevelController Current;
    public bool gameActive = false;
    public GameObject finishLine;
    public GameObject startMenu, gameMenu,finishMenu,gameOverMenu;
    public Slider levelProgressBar;
    int _currentLevel,_score;
    float _maxDistance;
    public Text startCoinText, finishCoinText, gameOverCoinText,scoreText,finishScoreText,bonusText;
    public Text currentLevelText, nextLevelText;
    
    
    private void Start()
    {
        Current = this;
        PlayerController.Current = GameObject.FindObjectOfType<PlayerController>();
        _currentLevel = PlayerPrefs.GetInt("currentLevel");

        currentLevelText.text = (_currentLevel + 1).ToString();
        nextLevelText.text = (_currentLevel + 2).ToString();
        UpdateCoinText();
    }

    private void Update()
    {
        if (gameActive)
        {
            float distance = finishLine.transform.position.z - PlayerController.Current.transform.position.z;
            levelProgressBar.value = 1 - (distance / _maxDistance);
        }
    }

    public void StartGame()
    {
        _maxDistance = finishLine.transform.position.z - PlayerController.Current.transform.position.z;
        PlayerController.Current.ChangeSpeed(PlayerController.Current.runningSpeed);
        startMenu.SetActive(false);
        gameMenu.SetActive(true);
        PlayerController.Current.animator.SetBool("run",true);
        gameActive = true;
    }

    public void RestartGame()
    {
        LevelLoader.Current.ChangeLevel(SceneManager.GetActiveScene().name);
    }

    public void LoadNextLevel()
    {
        LevelLoader.Current.ChangeLevel("Level " + (_currentLevel + 1)) ;
    }

    public void FinishGame(int bonus)
    {
        _score *= bonus;
        GiveMoneyToPlayer(_score);
        PlayerController.Current.animator.SetBool("win",true);
        PlayerController.Current.animator.SetBool("run",false);
        bonusText.text = bonus.ToString();
        finishScoreText.text = _score.ToString();
        gameMenu.SetActive(false);
        finishMenu.SetActive(true);
        gameActive = false;
    }

    public void GameOver()
    {
        PlayerController.Current.GetComponent<Rigidbody>().AddForce(Vector3.back * 50);
        UpdateCoinText();
        gameMenu.SetActive(false);
        gameOverMenu.SetActive(true);
        gameActive = false;
        PlayerController.Current.animator.SetBool("dead", true);
        PlayerController.Current.animator.SetBool("run", false);
    }

    public void ChangeScore(int value)
    {
        _score += value;
        scoreText.text = _score.ToString();
    }

    public void UpdateCoinText()
    {
        int coin = PlayerPrefs.GetInt("coin");
        startCoinText.text = coin.ToString();
        finishCoinText.text = coin.ToString();
        gameOverCoinText.text = coin.ToString();
    }

    public void GiveMoneyToPlayer(int value)
    {
        int coin = PlayerPrefs.GetInt("coin");
        coin += Mathf.Max(0, coin + value);
        PlayerPrefs.SetInt("coin", coin);
        UpdateCoinText();
    }
}
