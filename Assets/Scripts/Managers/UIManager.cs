using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [Header("Battle UI")]
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _scoreText;


    [Header("Buttons")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private Button startButton;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void UpdateBattleTimer(float time)
    {
        _timerText.text = time.ToString("0");
    }
    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }
    public void UpdateMoney(int score)
    {
        _moneyText.text = "" + score;
    }
}
