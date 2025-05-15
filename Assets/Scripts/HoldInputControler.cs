using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HoldInputControler : MonoBehaviour
{
    [SerializeField] private float _holdThreshold = 5.0f;
    [SerializeField] private float _delay = 5.0f;
    [SerializeField] private TouchControlls _inputActions;
    [SerializeField] private Slider _slider;
    private float holdTimer = 0.0f;
    private bool isHolding = false;


    private void Awake()
    {
        _inputActions = new TouchControlls();
        _slider.maxValue = _delay;
        _slider.value = _delay;
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
        if (isHolding)
        {
            holdTimer += Time.deltaTime;
            StartCoroutine(changeHui());
            if (holdTimer >= _holdThreshold)
            {
                isHolding = false;
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
        holdTimer = 0f;
        _delay = 5.0f;
        _slider.value = _delay;
        Debug.Log("Удержание прервано");
    }
    private IEnumerator changeHui()
    {
        _delay -= Time.deltaTime;
        _slider.value = _delay;
        yield return new WaitForSeconds(holdTimer);
        if (_delay <= 0)
        {
            _delay = 5.0f;
            StopCoroutine(changeHui());
        }
    }
}
