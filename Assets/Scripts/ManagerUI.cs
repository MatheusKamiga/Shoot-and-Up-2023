using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManagerUI : MonoBehaviour
{
    #region Singleton

    public static ManagerUI instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        timerText = GameObject.Find("Timer Text").GetComponent<TextMeshProUGUI>();
        gameOverWindow = GameObject.Find("GameOverWindow");
    }

    #endregion

    TextMeshProUGUI timerText;
    GameObject gameOverWindow;

    public void UpdateTimerText(float value)
    {
        timerText.text = value.ToString("0.0");
    }

    public void SetGameOver(bool active)
    {
        gameOverWindow.SetActive(active);
    }
}
