using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HoldInputControler : MonoBehaviour
{
    [SerializeField] private float _holdThreshold = 2.0f;
    [SerializeField] private float _delay = 2.0f;
    [SerializeField] private TouchControlls _inputActions;
    [SerializeField] private Slider _slider;
    [SerializeField] private PlayerController _player;
    [SerializeField] private float holdTimer = 0.0f;
    [SerializeField] private bool isHolding = false;
    public float Delay => _delay;


    private void Awake()
    {
        _inputActions = new TouchControlls();
        
    }
    public void setValue(GameObject slider)
    {
        _slider = slider.GetComponent<VehicleCellMovement>().SliderD;
        _slider.maxValue = _delay;
        _slider.value = _delay;
        _slider.enabled = true;
    }
    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.Action.Hold.started += OnHoldStarted;
        _inputActions.Action.Hold.canceled += OnHoldCanceled;
    }
    private void OnDisable()
    {
        _inputActions.Action.Hold.started -= OnHoldStarted;
        _inputActions.Action.Hold.canceled -= OnHoldCanceled;
        _inputActions.Disable();
    }
    private void Update()
    {
        if (isHolding && _slider != null)
        {
            holdTimer += Time.deltaTime;
            StartCoroutine(changeHui());
            if (holdTimer >= _holdThreshold)
            {
                isHolding = false;
                _player.SelectVehicle();
                Debug.Log("The Press is complete");
            }
        }
    }
    private void OnHoldStarted(InputAction.CallbackContext context)
    {
        holdTimer = 0f;
        isHolding = true;
        Debug.Log("Началось удержание");
    }

    private void OnHoldCanceled(InputAction.CallbackContext context)
    {
        
        isHolding = false;
        if (_slider == null) return;
        holdTimer = 0f;
        _delay = 2.0f;
        _slider.value = _delay;
        _slider.enabled = false;
        _slider = null;
        Debug.Log("Удержание прервано");
    }
    private IEnumerator changeHui()
    {
        if (_slider != null) 
        {
            _delay -= Time.deltaTime;
            _slider.value = _delay;
        }
        else
        {
            Debug.Log("Not assigned");
        }
        yield return new WaitForSeconds(holdTimer);
        if (_delay <= 0)
        {
            
            _delay = 2.0f;
            StopCoroutine(changeHui());
        }
    }
}
