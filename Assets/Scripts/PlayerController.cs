using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{


    [Header("Tools")]
    [SerializeField] private GameObject _airDeffense;
    [SerializeField] private GameObject _tower;
    public GameObject _selectedObject;
    [SerializeField] private string[] _nameZone;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private List<GameObject> _towers;
    [SerializeField] private List<GameObject> _airDefenses;

    [Header("Swipe Detects")]
    [SerializeField] private Transform _stabilizator;
    [SerializeField] private float _cameraSpeed;
    private TouchControlls _controls;
    private Camera _camera;
    private Transform _cameraTransform;
    private GameObject _choosedObject;
    private Coroutine _swipeCoroutine;

    [Header("Information")]
    private int indexX, indexY;
    private void Awake()
    {
        _controls = new TouchControlls();
        _camera = Camera.main;
        _cameraTransform = _camera.transform;
    }
    private void OnEnable()
    {
        _controls.Enable();
    }
    private void OnDisable()
    {
        _controls.Disable();
    }
    private void Start()
    {
        _controls.Touch.TouchPress.started += _ => SwipeStart();
        _controls.Touch.TouchPress.canceled += _ => SwipeEnd();
    }
   
    private void Update()
    {
        ObjectSelection();
        ObjectSetPosition();
        
        
    }
    private void SwipeStart()
    {
        _swipeCoroutine = StartCoroutine(SwipeDetection());
    }
    private void SwipeEnd()
    {
        StopCoroutine(_swipeCoroutine);
    }
    IEnumerator SwipeDetection()
    {
        float previousDistance = 0.0f, currentDistance = 0.0f;
        Debug.Log("Hello");
        while (true) 
        {
            currentDistance = Vector2.Distance(_stabilizator.position, _controls.Touch.PrimaryFingerPositon.ReadValue<Vector2>());
            //SwipeRight
            if (currentDistance > previousDistance && _cameraTransform.position.x > -4.6f)
            {
                Vector3 targetPosition = _cameraTransform.position;
                targetPosition.x -= 1;
                _cameraTransform.position = Vector3.Slerp(_cameraTransform.position, new Vector3(targetPosition.x, 17.5f, -12.0f), Time.deltaTime * _cameraSpeed);
            }
            //SwipeLeft
            if (currentDistance < previousDistance && _cameraTransform.position.x < 4.6f)
            {
                Vector3 targetPosition = _cameraTransform.position;
                targetPosition.x += 1;
                _cameraTransform.position = Vector3.Slerp(_cameraTransform.position, new Vector3(targetPosition.x, 17.5f, -12.0f), Time.deltaTime * _cameraSpeed);
            }
            previousDistance = currentDistance;
            yield return null;
        }
    }
    private void ObjectSelection()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == UnityEngine.TouchPhase.Began)
            {
                Ray ray = _camera.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && hit.collider != null)
                {
                    Debug.Log("Нажали на объект: " + hit.collider.gameObject.name);
                    if (_selectedObject) _selectedObject = null;
                    _selectedObject = hit.collider.gameObject;
                    _selectedObject.gameObject.GetComponent<SixAngelSelection>().ActiveObject.SetActive(false);
                    _selectedObject.gameObject.GetComponent<SixAngelSelection>().UnactiveObject.SetActive(true);
                    _selectedObject.gameObject.GetComponent<SixAngelSelection>().tmpSelectedObject = gameObject;
                    

                }
            }

        }

    }
    private void ObjectSetPosition()
    {
        bool isPressed = Input.touchCount > 0;
        bool isNotNull = isPressed && _selectedObject != null && _selectedObject.tag == _nameZone[0];
        bool TowerCanBuilt = _towers.Count > 1 && _gameManager.SelectedObject == "0";
        bool AirDeffenseCanBuilt = _airDefenses.Count > 3 && _gameManager.SelectedObject == "1";
        bool isAssigned = _gameManager.SelectedObject != null;
        Vector3 towerOffset = new Vector3(0,0,0);
        switch (_gameManager.SelectedObject)
        {
            case "0": _choosedObject = _tower; break;
            case "1": _choosedObject = _airDeffense; break;
        }
        
        if (isNotNull)
        {
            towerOffset = new Vector3(_selectedObject.transform.position.x, 0.3f, _selectedObject.transform.position.z);
            Touch touch = Input.GetTouch(0);
            if (touch.phase == UnityEngine.TouchPhase.Began)
            {
                if (_towers.Count > 0 && _gameManager.SelectedObject == "0")
                {
                    if (_gameManager.FightIsStarted) return;
                    _towers[0].gameObject.transform.position = towerOffset;
                    _towers[0].GetComponent<Tower>().indexX = _selectedObject.GetComponent<SixAngelSelection>().IndexX;
                    _towers[0].GetComponent<Tower>().indexY = _selectedObject.GetComponent<SixAngelSelection>().IndexY;
                }
                else
                {
                    GameObject newBuild = Instantiate(_choosedObject, towerOffset, Quaternion.identity);
                    switch (_gameManager.SelectedObject)
                    {
                        case "0": 
                            if (_towers.Count <= 0) _towers.Add(newBuild);
                            _towers[0].GetComponent<Tower>().indexX = _selectedObject.GetComponent<SixAngelSelection>().IndexX;
                            _towers[0].GetComponent<Tower>().indexY = _selectedObject.GetComponent<SixAngelSelection>().IndexY; break;
                        case "1": _airDefenses.Add(newBuild); break;
                    }
                }
                
            }

        }
        else return;
        if(TowerCanBuilt)
        {
            Destroy(_towers[0]);
            _towers.RemoveAt(0);
        }
        if (AirDeffenseCanBuilt)
        {
            Destroy(_airDefenses[0]);
            _airDefenses.RemoveAt(0);
        }

    }

}
