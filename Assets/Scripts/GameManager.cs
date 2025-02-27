using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Canvas _UI;
    [SerializeField] private GameObject _towerButton;
    [SerializeField] private GameObject _startButton;
    [SerializeField] private MissleSpawner _missleSpawner;
    public bool isGameActive;
    public bool FightIsStarted;
    public string SelectedObject = "0";
    [SerializeField] private PlayerController _player;
    public List<Transform> Towers;
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
        _missleSpawner.spawnStart();
        _towerButton.SetActive(false);
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

}
