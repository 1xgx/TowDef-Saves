using System.Collections.Generic;
using UnityEngine;
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
    //[SerializeField] private float minimumDistance = .2f;
    //[SerializeField] private float maximumTime = 1.0f;
    private Vector2 _startPosition;
    private float _startTime;
    private Vector2 _endPosition;
    private float _endTime;
    private InputManager _inputManager;
    private Camera _camera;
    private GameObject choosedObject;
    [Header("Information")]
    private int indexX, indexY;
    private void Awake()
    {
        //_inputManager = InputManager.Instance;
        _camera = Camera.main;
    }
    /*
    private void OnEnable()
    {
        _inputManager.onStartTouch += SwipeStart;
        _inputManager.onEndTouch += SwipeEnd;
    }
    private void OnDisable()
    {
        _inputManager.onStartTouch -= SwipeStart;
        _inputManager.onEndTouch -= SwipeEnd;
    }
    private void SwipeStart(Vector2 Position, float time)
    {
        _startPosition = Position;
        _startTime = time;
    }
    private void SwipeEnd(Vector2 Position, float time)
    {
        _endPosition = Position;
        _endTime = time;
        DetectSwipe();
    }
    private void DetectSwipe()
    {
        if (Vector2.Distance(_startPosition, _endPosition) >= minimumDistance && (_endTime - _startTime) <= maximumTime)
        {
            Debug.DrawLine(_startPosition, _endPosition, Color.red, 5.0f);
        }
    } */
    void Update()
    {
        ObjectSelection();
        ObjectSetPosition();
        
        
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
            case "0": choosedObject = _tower; break;
            case "1": choosedObject = _airDeffense; break;
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
                    GameObject newBuild = Instantiate(choosedObject, towerOffset, Quaternion.identity);
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
