using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] GameObject uiPanel;
    [SerializeField] Button resetButton;
    [SerializeField] GameObject gameOverUIPanel;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float winTime = 60f;

    Coroutine timerCoroutine;
    bool gameOver;
    void Start()
    {
        gameOver = false;
        timerText.text = winTime.ToString();
        startButton.onClick.AddListener(StartGame);
        resetButton.onClick.AddListener(ResetGame);
        uiPanel.SetActive(true);
        gameOverUIPanel.SetActive(false);
    }

    void StartGame()
    {
        uiPanel.SetActive(false);
        timerCoroutine = StartCoroutine(TimerCoroutine());
        ObstacleManager.Instance.StartObstacles();
    }

    IEnumerator TimerCoroutine()
    {
        WaitForSeconds oneSecondDelay = new WaitForSeconds(1f);
        while (winTime > 0)
        {
            winTime -= 1f;
            timerText.text = winTime.ToString();
            yield return oneSecondDelay;
        }
        ObstacleManager.Instance.StopObstacles();
        gameOver = true;
        yield return oneSecondDelay;
        timerText.text = "WIN!";
        GameOver(true);
    }

    public bool GameOverGetter()
    {
        return gameOver;
    }

    public void GameOver(bool win)
    {
        gameOver = true;
        gameOverText.text = win ? "Congratulations!\nYou Won!" : "Whoops!\nYou Lost!";
        gameOverUIPanel.SetActive(true);
        if(timerCoroutine != null) StopCoroutine(timerCoroutine);
    }

    void ResetGame()
    {
        //reload game for reset
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
