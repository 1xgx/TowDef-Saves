using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Canvas _UI;
    [SerializeField] private GameObject _towerButton;
    [SerializeField] private GameObject _startButton;
    [SerializeField] private MissleSpawner _missleSpawner;
    [SerializeField] private PlayerController _playerContorler;
    [SerializeField] private Missle[] _allMissles; 
    public bool isGameActive;
    public bool FightIsStarted;
    public string SelectedObject = "0";
    [SerializeField] private PlayerController _player;
    public List<Transform> Towers;
    [SerializeField] private float _delayLevelStart = 3.0f;
    [SerializeField] private float _delayBeforeStart = 15.0f;
    [SerializeField] private float _timeOfBattle = 120.0f;
    [SerializeField] private int _wavesOfBattle = 1;
    [SerializeField] private TextMeshProUGUI _timer;
    [SerializeField] private GameObject LevelPanel;
    
    private void Start()
    {
        Invoke(nameof(StartLevel), _delayLevelStart);
    }
    private void StartLevel()
    {
        Destroy(LevelPanel);
        StartCoroutine(startWave(_delayBeforeStart));
    }
    public void restartGame()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void startGame()
    {
        isGameActive = true;
        _startButton.SetActive(true);
        
    }
    public void startPlayerGame()
    {
        FightIsStarted = true;
        _missleSpawner.spawnStart(gameObject.GetComponent<GameManager>(), _wavesOfBattle);
        
        _towerButton.SetActive(false);
    }
    public void sendMoney(int value)
    {
        _playerContorler._money += value;
        _playerContorler.UpdateMoney();
    }
    private void NextWave()
    {
        _wavesOfBattle++;
        if (_wavesOfBattle < 4) StartCoroutine(startWave(_delayBeforeStart));
        else return;
        _playerContorler.enabled = true;
    }
    public void StopWave()
    {
        FightIsStarted = false;
        _missleSpawner.enabled = false;
        _playerContorler.enabled = false;
        _allMissles = FindObjectsOfType<Missle>();
        foreach (Missle missle in _allMissles)
        {
            Destroy(missle.gameObject);
        }
        NextWave();

    }
    public void BuildSelection(string message)
    {
        SelectedObject = message;
        Debug.Log(message);
        _player.ObjectSetPositionOnHexagon(message);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public Transform randomElectroStationForMissle() 
    {
        Transform TargetObject = Towers[Random.Range(0,Towers.Count)];

        return TargetObject;
    }

    private IEnumerator startWave(float delay)
    {
        while (delay > 0)
        {
            UpdateDisplayTimer(delay);
            yield return new WaitForSeconds(1.0f);
            delay--;

        }
        delay = 0;
        UpdateDisplayTimer(delay);
        StartCoroutine(Battle(_timeOfBattle));
        StopCoroutine(startWave(0.0f));
        
    }
    private void UpdateDisplayTimer(float CurrentTime)
    {
        if (CurrentTime == 0) startPlayerGame();
        _timer.text = "" + CurrentTime + "";
    }
    private IEnumerator Battle(float time)
    {
        while(time > 0)
        {
            UpdateDisplayTimer(time);
            yield return new WaitForSeconds(1.0f);
            time--;
        }
        time = 0;
        _timer.text = "" + time + "";
        if (time == 0) StopWave();
        StopCoroutine(Battle(0.0f));
    }
    
}
