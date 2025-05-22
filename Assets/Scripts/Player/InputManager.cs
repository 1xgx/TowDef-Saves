using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : Singelton<InputManager>
{
    
    #region Events
    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent onStartTouch;
    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent onEndTouch;
    #endregion
    [Header("Touch Controls")]
    private TouchControlls _touchControlls;
    private Camera _camera; 
    private void Awake()
    {
        _touchControlls = new TouchControlls();
        _camera = Camera.main;
    }
    private void OnEnable()
    {
        _touchControlls.Enable();
    }
    private void OnDisable()
    {
        _touchControlls.Disable();
    }
    private void Start()
    {
        _touchControlls.Touch.TouchPress.started += ctx => StartTouch(ctx);
        _touchControlls.Touch.TouchPress.canceled += ctx => EndTouch(ctx);
    }
    
    private void StartTouch(InputAction.CallbackContext context)
    {
        if (onStartTouch != null) onStartTouch(Utils.ScreenToWorld(_camera, _touchControlls.Touch.TouchPosition.ReadValue<Vector2>()), (float)context.startTime);

    }
    private void EndTouch(InputAction.CallbackContext context)
    {
        if (onEndTouch != null) onStartTouch(Utils.ScreenToWorld(_camera, _touchControlls.Touch.TouchPosition.ReadValue<Vector2>()), (float)context.time);
    }
    public Vector2 PrimaryPosition()
    {
        return Utils.ScreenToWorld(_camera, _touchControlls.Touch.TouchPosition.ReadValue<Vector2>());
    }
}
