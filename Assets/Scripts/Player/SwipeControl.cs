using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeControl : MonoBehaviour
{
    [SerializeField] private InputAction _swipeAction;
    [SerializeField] private float _swipeThreashold = 50.0f;
    [SerializeField] private float _movementSpeed = 5.0f;
    (float, float) _offsetX;
    private Camera _camera;

    private Vector2 _startTouchPosition;
    private Vector2 _endTouchPosition;
    private bool _isSwiping = false;

    private void Awake()
    {
        _camera = Camera.main;
        _offsetX.Item1 = -5.0f;
        _offsetX.Item2 = 6.8f;
    }
    private void OnEnable()
    {
        _swipeAction.performed += OnSwipePerformed;
        _swipeAction.canceled += OnSwipeCancled;
        _swipeAction.Enable();


    }
    private void OnDisable()
    {
        _swipeAction.performed -= OnSwipePerformed;
        _swipeAction.canceled -= OnSwipeCancled;
        _swipeAction.Disable();
    }
    private void OnSwipePerformed(InputAction.CallbackContext context)
    {
        if (!_isSwiping)
        {
            _startTouchPosition = context.ReadValue<Vector2>();
            _isSwiping = true;
        }
    }
    private void OnSwipeCancled(InputAction.CallbackContext context)
    {
        if (_isSwiping)
        {
            _endTouchPosition = context.ReadValue<Vector2>();
            _isSwiping = false;
            ProcessSwipe();
        }
    }
    private void ProcessSwipe()
    {
        float swipeDistance = _endTouchPosition.x - _startTouchPosition.x;
        if (Mathf.Abs(swipeDistance) >= _swipeThreashold)
        {
            if (swipeDistance > 0)
            {
                MoveObject(Vector3.right);
            }
            else
            {
                MoveObject(Vector3.left);
            }
        }
    }
    private void MoveObject(Vector3 direction)
    {
        if (transform.position.x > 6.8f)
        {
            _camera.transform.position = new Vector3(_offsetX.Item2, _camera.transform.position.y,_camera.transform.position.z);
        }
        else if (transform.position.x < -5)
        {
            _camera.transform.position = new Vector3(_offsetX.Item1, _camera.transform.position.y, _camera.transform.position.z);
            
        }
        else
        {
            _camera.transform.position += direction * _movementSpeed * Time.deltaTime;
        }
    }
}
