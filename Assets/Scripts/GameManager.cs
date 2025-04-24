using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Canvas _UI;
    [SerializeField] private MissleSpawner _missleSpawner;
    [SerializeField] private PlayerController _playerContorler;
    [SerializeField] private Missle[] _allMissles; 
    public bool isGameActive;
    public bool FightIsStarted;
    public string SelectedObject = "0";
    [SerializeField] private PlayerController _player;
    public List<Transform> Towers;
    [SerializeField] private float _delayLevelStart = 3.0f;
    [SerializeField] private float _breakDuration = 15.0f;
    [SerializeField] private float _waveDuration = 10.0f;
    [SerializeField] private int _currentWave = 1;
    [SerializeField] private TextMeshProUGUI _timer;
    [SerializeField] private GameObject LevelPanel;
    [Header("Sound Effect Script")]
    [SerializeField] private SoundEffectsSc _soundEffectsSc;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _delayOfClick = 0.5f;
    [Header("Day and Night cycle")]
    [SerializeField] private Light _directionalLight;
    [SerializeField] private Material _skyBoxNight;
    [SerializeField] private Material _skyBoxDay;
    [SerializeField] private Color _dayColor = Color.white;
    [SerializeField] private Color _nightColor = new Color(0.1f,0.1f,0.2f);
    [SerializeField] private float _transitionDuration = 3.0f;
    public List<NightLight> NightLights;
    [SerializeField] private bool NightTime = false;

    private void Start()
    {
        Invoke(nameof(StartLevel), _delayLevelStart);
    }
    private void StartLevel()
    {
        Destroy(LevelPanel);
        StartCoroutine(startWave(_breakDuration));
    }
    public void restartGame()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void startGame()
    {
        isGameActive = true;
        //_startButton.SetActive(true);
        
    }
    public void startPlayerGame()
    {
        FightIsStarted = true;
        _missleSpawner.spawnStart(gameObject.GetComponent<GameManager>(), _currentWave);
    }
    public void sendMoney(int value)
    {
        _playerContorler._money += value;
        _playerContorler.UpdateMoney();
    }
    public void DieSound(ObjectType Type)
    {
        //_soundEffectsSc.Play(Type);
    }
    private void NextWave()
    {
        _currentWave++;
        if (_currentWave < 4) StartCoroutine(startWave(_breakDuration));
        else return;
        if (_currentWave == 2) ChangeToNight();
        if (_currentWave == 3) ChangeToDay();
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
        _audioSource.PlayOneShot(_audioClip);
        StartCoroutine(Delay());
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
        StartCoroutine(Battle(_waveDuration));
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
    private void ChangeToNight()
    {
        //Changing since Day to Night
        StartCoroutine(TransitionToNight());
    }
    private void ChangeToDay()
    {
        StartCoroutine(TransitionToDay());
    }
    IEnumerator TransitionToNight()
    {
        NightTime = true;
        for (int i = 0; i < NightLights.Count; i++)
        {
            NightLights[i].Night(NightTime);
        }
        float t = 0f;

        // Save the current parametrs of light
        Color initialColor = _directionalLight.color;
        float initialIntensity = _directionalLight.intensity;

        // Step by step change the light and sky box to night
        while (t < 1f)
        {
            t += Time.deltaTime / _transitionDuration;
            _directionalLight.color = Color.Lerp(initialColor, _nightColor, t);
            _directionalLight.intensity = Mathf.Lerp(initialIntensity, 0.2f, t);
            UnityEngine.RenderSettings.skybox.Lerp(_skyBoxDay, _skyBoxNight, t);

            yield return null;
        }

        // Set-up final results
        _directionalLight.color = _nightColor;
        _directionalLight.intensity = 0.2f;
        UnityEngine.RenderSettings.skybox = _skyBoxNight;
    }
    IEnumerator TransitionToDay()
    {
        NightTime = false;
        for(int i = 0; i<NightLights.Count; i++)
        {
            NightLights[i].Night(NightTime);
        }
        float t = 0f;

        // Save the current parametrs of light
        Color initialColor = _directionalLight.color;
        float initialIntensity = _directionalLight.intensity;

        // Step by step change the light and sky box to night
        while (t < 1f)
        {
            t += Time.deltaTime / _transitionDuration;
            _directionalLight.color = Color.Lerp(initialColor, _dayColor, t);
            _directionalLight.intensity = Mathf.Lerp(initialIntensity, 1f, t);
            UnityEngine.RenderSettings.skybox.Lerp(_skyBoxNight, _skyBoxDay, t);

            yield return null;
        }

        // Set-up final results
        _directionalLight.color = _dayColor;
        _directionalLight.intensity = 1f;
        UnityEngine.RenderSettings.skybox = _skyBoxDay;
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(_delayOfClick);
    }
}
