using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    #region Singleton
    public static UIManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion
    [SerializeField] Text scoreUI, finalScoreUI, recordUI, newRecordUI;
    [SerializeField] GameObject gameOverWindow, rewardWindow;

    public GameObject RewardWindow { get => rewardWindow; }

    public void FinishMatch()
    {
        GameManager.instance.FinishMatch();
    }

    public void RequestReward()
    {
        //GameManager.instance.ResquestAdReward();
    }

    public void GameOver(int score, int record, bool newRecord)
    {
        gameOverWindow.SetActive(true);
        finalScoreUI.text = "Pontuação: " + score.ToString("0000");
        recordUI.text = "Recorde: " + record.ToString("0000");
        newRecordUI.gameObject.SetActive(newRecord);
    }

    public void LoadScene(string sceneName)
    {
        GameManager.instance.LoadScene(sceneName);
    }

    public void UpdateScore(int value)
    {
        scoreUI.text = "Score: " + value.ToString("0000");
    }
}
