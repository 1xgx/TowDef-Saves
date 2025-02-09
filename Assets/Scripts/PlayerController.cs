using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
public class PlayerController : MonoBehaviour
{


    readonly private string MilitaryObject = "Military_Object";
    [Header("Build")]
    [SerializeField] private GameObject _airDeffense;
    [SerializeField] private GameObject _tower;
    [SerializeField] private GameObject _radar;
    [SerializeField] private GameObject _gepard;//Test
    [SerializeField] private GameObject _mobileGroup;//Test
    public GameObject _selectedObject;
    [SerializeField] private GameObject _tmpSelectedObject;
    [SerializeField] private GameObject _tmpCellSelectedObject;
    [SerializeField] private List<GameObject> _AllReservedCell;
    [SerializeField] private GameObject _geoPoint;
    [SerializeField] private GameObject _geoPointRef;
    [SerializeField] private GameObject _cells;
    [SerializeField] private GameObject[,] _hexagonGrid;
    [Header("Tools")]
    [SerializeField] private string[] _nameZone;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private bool _selectMode = false;
    [SerializeField] private bool _builtMode = true;
    [SerializeField] private TextMeshProUGUI _timer;
    private float _currentTime = 20.0f;

    [Header ("List of Builds")]
    [SerializeField] private List<GameObject> _towers;
    [SerializeField] private List<GameObject> _airDefenses;
    [SerializeField] private List<GameObject> _radars;
    [SerializeField] private List<GameObject> _gepards;//Test list
    [SerializeField] private List<GameObject> _mobileGroups;//Test list
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

    [SerializeField] private GameObject _battleMenu; //Test Variant
    
    private void Awake()
    {
        _controls = new TouchControlls();
        _camera = Camera.main;
        _cameraTransform = _camera.transform;
    }
    private void Start()
    {
        SpawnElectroStation();
        StartCoroutine(Timer());
    }
    IEnumerator Timer()
    {
        while (_currentTime > 0)
        {
            UpdateDisplayTimer(_currentTime);
            yield return new WaitForSeconds(1.0f);
            _currentTime--;
        }
        _currentTime = 0;
        UpdateDisplayTimer(_currentTime);
    }
    private void UpdateDisplayTimer(float CurrentTime)
    {
        if (_currentTime == 0) _gameManager.startPlayerGame();
        _timer.text = "Time: " + _currentTime + "S";
    }
    private void SpawnElectroStation()
    {
        _hexagonGrid = _cells.GetComponent<GridCell>()._hexagonGrid;
        int x = Random.Range(0, _hexagonGrid.GetLength(0));
        int y = Random.Range(0, _hexagonGrid.GetLength(1));

        if (_hexagonGrid[x, y].tag == "Zone")
        {
            GameObject NewBuild = Instantiate(_tower, new Vector3(_hexagonGrid[x, y].transform.position.x, .1f, _hexagonGrid[x,y].transform.position.z), Quaternion.identity);
            
            if (_towers.Count <= 0) _towers.Add(NewBuild); _AllReservedCell.Add(NewBuild);
            _towers[0].GetComponent<Tower>().indexX = _hexagonGrid[x, y].GetComponent<SixAngelSelection>().IndexX;
            _towers[0].GetComponent<Tower>().indexY = _hexagonGrid[x, y].GetComponent<SixAngelSelection>().IndexY;
        }
        else
        {
            x = Random.Range(0, _hexagonGrid.GetLength(0));
            y = Random.Range(0, _hexagonGrid.GetLength(1));
            SpawnElectroStation();
        }
        
    }
    private void OnEnable()
    {
        _controls.Enable();
    }
    private void OnDisable()
    {
        _controls.Disable();
    }
   
    private void Update()
    {
        _controls.Touch.TouchPress.started += _ => SwipeStart();
        _controls.Touch.TouchPress.canceled += _ => SwipeEnd();
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
               
                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    position = touch.position
                };
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                if(results.Count > 0)
                {
                    return;
                }
                Ray ray = _camera.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && hit.collider != null)
                {
                    Debug.Log("Нажали на объект: " + hit.collider.gameObject.name);
                    if (_selectedObject) _selectedObject = null;
                    _selectedObject = hit.collider.gameObject;
                    if (_selectedObject.tag == "Plane")
                    {
                        _selectedObject = null;
                        return;
                    }
                    if (_selectedObject != null && _selectedObject.tag == "DeadZone" || _selectedObject.tag == "Zone" || _selectedObject.tag == "WaterZone")
                    {
                        _selectedObject.gameObject.GetComponent<SixAngelSelection>().ActiveObject.SetActive(false);
                        _selectedObject.gameObject.GetComponent<SixAngelSelection>().UnactiveObject.SetActive(true);
                        _selectedObject.gameObject.GetComponent<SixAngelSelection>().tmpSelectedObject = gameObject;
                        if (_geoPointRef != null) _geoPointRef.transform.position = new Vector3(_selectedObject.transform.position.x,0,_selectedObject.transform.position.z);
                        _battleMenu.SetActive(true);
                        _battleMenu.transform.position = new Vector3(_selectedObject.transform.position.x, 1.0f, _selectedObject.transform.position.z);


                    }
                    else if(_selectedObject.tag == MilitaryObject && _gameManager.FightIsStarted)
                    {
                        
                        _selectedObject.transform.localScale = new Vector3(_selectedObject.transform.localScale.x + .1f, _selectedObject.transform.localScale.y + .1f, _selectedObject.transform.localScale.z + .1f);
                        _geoPointRef = Instantiate(_geoPoint, new Vector3(_selectedObject.transform.position.x, _selectedObject.transform.position.y + 1.0f, _selectedObject.transform.position.z), Quaternion.identity);
                        _tmpSelectedObject = _selectedObject;
                        _gameManager.SelectedObject = "nothing";
                        _selectMode = true;
                        _builtMode = false;
                    }
                    else
                    {
                        return;
                    }
                    

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
        bool RadarCanBuilt = _radars.Count > 3 && _gameManager.SelectedObject == "2";
        bool GepardCanBuilt = _gepards.Count > 1 && _gameManager.SelectedObject == "TestGepard";
        bool MGroupCanBuilt = _mobileGroups.Count > 1 && _gameManager.SelectedObject == "TestMGroup";
        


        bool isAssigned = _gameManager.SelectedObject != null;
        Vector3 towerOffset = new Vector3(0,0,0);
        switch (_gameManager.SelectedObject)
        {
           // case "0": _choosedObject = _tower; break;
            case "1": _choosedObject = _airDeffense; break;
            case "2":_choosedObject = _radar; break;
            case "TestGepard": _choosedObject = _gepard; break;
            case "TestMGroup": _choosedObject = _mobileGroup; break;
            case "Go":
                if(_selectedObject != null && _tmpSelectedObject != null)
                {
                    VehicleCellMovement tmpVehicleCellMovement = _tmpSelectedObject.GetComponent<VehicleCellMovement>();
                    SixAngelSelection tmpHexagon = _selectedObject.GetComponent<SixAngelSelection>();
                    tmpVehicleCellMovement.GetChoosedHexagon(tmpHexagon.IndexX, tmpHexagon.IndexY, _selectedObject.transform, true);
                    tmpVehicleCellMovement.IndexX = tmpHexagon.IndexX;
                    tmpVehicleCellMovement.IndexY = tmpHexagon.IndexY;
                    tmpVehicleCellMovement.VehiclePosition.Item1 = tmpHexagon.IndexX;
                    tmpVehicleCellMovement.VehiclePosition.Item2 = tmpHexagon.IndexY;
                    _selectedObject.GetComponent<SixAngelSelection>().ReferenceOfObject = _tmpSelectedObject;
                    _tmpCellSelectedObject.GetComponent<SixAngelSelection>().ReferenceOfObject = null;
                    _tmpCellSelectedObject = _selectedObject;
                    _tmpSelectedObject = null;
                    _builtMode = true;
                    _selectMode = false;
                    Destroy(_geoPointRef);
                }
                break;
        }
        

        if (isNotNull)
        {
            GameObject ReferenceFromSelectedObject = _selectedObject.GetComponent<SixAngelSelection>().ReferenceOfObject;
            towerOffset = new Vector3(_selectedObject.transform.position.x, 0.3f, _selectedObject.transform.position.z);
            Touch touch = Input.GetTouch(0);
            if (touch.phase == UnityEngine.TouchPhase.Began)
            {
                if (ReferenceFromSelectedObject != null)
                {
                    Debug.Log($"The {_selectedObject} is busy");
                    return;
                }
                else
                {

                    //if (_towers.Count > 0 && _gameManager.SelectedObject == "0")
                    //{
                    //    if (_gameManager.FightIsStarted) return;
                    //    _towers[0].gameObject.transform.position = towerOffset;
                    //    _towers[0].GetComponent<Tower>().indexX = _selectedObject.GetComponent<SixAngelSelection>().IndexX;
                    //    _towers[0].GetComponent<Tower>().indexY = _selectedObject.GetComponent<SixAngelSelection>().IndexY;
                    //}
                    //else
                    if (_tmpSelectedObject != null && _tmpSelectedObject.tag == MilitaryObject)//Test Gepard and MobileGroup
                    {
                        _tmpSelectedObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        
                        
                    }
                    else
                    {
                        if (_choosedObject != null)
                        {
                            GameObject newBuild = Instantiate(_choosedObject, towerOffset, Quaternion.identity);
                            _selectedObject.GetComponent<SixAngelSelection>().ReferenceOfObject = newBuild;

                            if (_builtMode == true)
                            {
                                switch (_gameManager.SelectedObject)
                                {
                                    //case "0":
                                    //    if (CheckSelectedObject(_towers, _selectedObject)) return;
                                    //    if (_towers.Count <= 0) _towers.Add(newBuild); _AllReservedCell.Add(newBuild);
                                    //    _towers[0].GetComponent<Tower>().indexX = _selectedObject.GetComponent<SixAngelSelection>().IndexX;
                                    //    _towers[0].GetComponent<Tower>().indexY = _selectedObject.GetComponent<SixAngelSelection>().IndexY;
                                    //    break;
                                    case "1":
                                        if (CheckSelectedObject(_airDefenses, _selectedObject)) return;
                                        _airDefenses.Add(newBuild); _AllReservedCell.Add(newBuild);
                                        if (_radars.Count > 0)
                                        {
                                            for (int i = 0; i < _radars.Count; i++)
                                            {
                                                Debug.Log(newBuild.name + "");
                                                _radars[i].GetComponent<RadarDetection>()._airDefenses.Add(newBuild.GetComponent<AirDefenseController>());
                                            }
                                        }

                                        break;
                                    case "2":
                                        if (CheckSelectedObject(_radars, _selectedObject)) return;
                                        _radars.Add(newBuild); _AllReservedCell.Add(newBuild);
                                        break;
                                    case "TestGepard": //Test list

                                        newBuild.GetComponent<VehicleCellMovement>().VehiclePosition.Item1 = _selectedObject.GetComponent<SixAngelSelection>().IndexX;
                                        newBuild.GetComponent<VehicleCellMovement>().VehiclePosition.Item2 = _selectedObject.GetComponent<SixAngelSelection>().IndexY;
                                        newBuild.GetComponent<VehicleCellMovement>().IndexX = _selectedObject.GetComponent<SixAngelSelection>().IndexX;
                                        newBuild.GetComponent<VehicleCellMovement>().IndexY = _selectedObject.GetComponent<SixAngelSelection>().IndexY;
                                        newBuild.GetComponent<VehicleCellMovement>().Spawn();
                                        _tmpCellSelectedObject = _selectedObject;
                                        _AllReservedCell.Add(newBuild);
                                        _mobileGroups.Add(newBuild);
                                        break;
                                    case "TestMGroup":
                                        newBuild.GetComponent<VehicleCellMovement>().VehiclePosition.Item1 = _selectedObject.GetComponent<SixAngelSelection>().IndexX;
                                        newBuild.GetComponent<VehicleCellMovement>().VehiclePosition.Item2 = _selectedObject.GetComponent<SixAngelSelection>().IndexY;
                                        newBuild.GetComponent<VehicleCellMovement>().IndexX = _selectedObject.GetComponent<SixAngelSelection>().IndexX;
                                        newBuild.GetComponent<VehicleCellMovement>().IndexY = _selectedObject.GetComponent<SixAngelSelection>().IndexY;
                                        newBuild.GetComponent<VehicleCellMovement>().Spawn();
                                        _tmpCellSelectedObject = _selectedObject;
                                        _AllReservedCell.Add(newBuild);
                                        _mobileGroups.Add(newBuild);
                                        break;
                                }
                            }
                        }
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
        if (RadarCanBuilt)
        {
            Destroy(_radars[0]);
            _radars.RemoveAt(0);
        }
        if (GepardCanBuilt)
        {
            Destroy(_gepards[0]);
            _gepards.RemoveAt(0);
        }

    }
    private bool CheckSelectedObject(List<GameObject> Objects, GameObject SelectedObject)
    {
        foreach(var OneObject in Objects)
        {
            if(SelectedObject == OneObject)
            {
                Debug.Log($"{SelectedObject} - same object");
                return true;
            }
        }
        return false;
    }

}
