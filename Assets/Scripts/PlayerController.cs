using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
public class PlayerController : MonoBehaviour
{


    readonly private string MilitaryObject = "Military_Object";
    [Header("Build")]
    [SerializeField] private GameObject _airDeffense;
    [SerializeField] private GameObject _electroStation;
    [SerializeField] private List<GameObject> _subElectroStations;
    [SerializeField] private GameObject _radar;
    [SerializeField] private GameObject _ecs;
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
    public int _money = 100;
    [SerializeField] private TextMeshProUGUI _textOfMoney;
    [Header("Tools")]
    [SerializeField] private string[] _nameZone;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private bool _selectMode = false;
    [SerializeField] private bool _builtMode = true;
    [SerializeField] private bool _AdminBool = false;
    [SerializeField] private TextMeshProUGUI _timer;
    [SerializeField] private float _currentTime = 20.0f;

    [Header ("List of Builds")]
    [SerializeField] private List<GameObject> _electroStations;
    [SerializeField] private List<GameObject> _subElectroStationList;
    [SerializeField] private List<GameObject> _airDefenses;
    [SerializeField] private List<AirDefenseController> _airDefensesControler;
    [SerializeField] private List<GameObject> _radars;
    [SerializeField] private List<GameObject> _gepards;//Test list
    [SerializeField] private List<GameObject> _mobileGroups;//Test list
    [SerializeField] private List<GameObject> _ecss;
    [Header("Swipe Detects")]
    [SerializeField] private Transform _stabilizator;
    [SerializeField] private float _cameraSpeed;
    private TouchControlls _controls;
    private Camera _camera;
    private Transform _cameraTransform;
    [SerializeField] private GameObject _choosedObject;
    private Coroutine _swipeCoroutine;

    [Header("Information")]
    private int indexX, indexY;

    [SerializeField] private GameObject _battleMenu; //Test Variant
    //[SerializeField] private GameObject _subButttleMenu; //Test Variant
    
    private void Awake()
    {
        _controls = new TouchControlls();
        _camera = Camera.main;
        _cameraTransform = _camera.transform;
    }
    private void Start()
    {
        UpdateMoney();
       // SpawnElectroStation();
       // SpawnSubElectroStation();
        //StartCoroutine(Timer());
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
    public void UpdateMoney()
    {
        _textOfMoney.text = "" + _money;
    }
    private void Update()
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
                    if (_selectedObject)
                    {
                        _selectedObject = null;
                    }
                    _selectedObject = hit.collider.gameObject;
                    if (_selectedObject.tag == "Plane")
                    {
                        _selectedObject = null;
                        return;
                    }
                    if (_selectedObject != null && _selectedObject.tag == "DeadZone" || _selectedObject.tag == "Zone" || _selectedObject.tag == "WaterZone")
                    {
                        SixAngelSelection tmpSixAngelSelection = _selectedObject.gameObject.GetComponent<SixAngelSelection>();
                        tmpSixAngelSelection.ActiveObject.SetActive(false);
                        tmpSixAngelSelection.UnactiveObject.SetActive(true);
                        tmpSixAngelSelection.tmpSelectedObject = gameObject;
                        if (_geoPointRef != null && _selectedObject.tag != "DeadZone" && tmpSixAngelSelection.ReferenceOfObject == null)
                        {
                            _geoPointRef.transform.position = new Vector3(_selectedObject.transform.position.x, 0, _selectedObject.transform.position.z);
                            UnityEngine.UI.Button newBut = _battleMenu.GetComponent<BattleMenu>()._buttons[2].GetComponent<BattleMenuSubButtons>().BattleMenu[3].GetComponent<UnityEngine.UI.Button>();
                            newBut.interactable = true;


                        }
                        else
                        {
                            UnityEngine.UI.Button newBut = _battleMenu.GetComponent<BattleMenu>()._buttons[2].GetComponent<BattleMenuSubButtons>().BattleMenu[3].GetComponent<UnityEngine.UI.Button>();
                            newBut.interactable = false;
                        }
                        _battleMenu.SetActive(true);
                        _battleMenu.transform.position = new Vector3(_selectedObject.transform.position.x, 2.4f, _selectedObject.transform.position.z);


                    }
                    else if(_selectedObject.tag == MilitaryObject && _geoPointRef != null)
                    {
                        return;
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
    public void ObjectSetPositionOnHexagon(string message)
    {
        bool isNotNull = _selectedObject != null && _selectedObject.tag == _nameZone[0];
        Vector3 towerOffset = new Vector3(0, 0, 0);
        switch (message)
        {
            case "1": _choosedObject = _airDeffense; break;
            case "2": _choosedObject = _radar; break;
            case "3": _choosedObject = _ecs; break;
            case "TestGepard": _choosedObject = _gepard; break;//Test
            case "TestMGroup": _choosedObject = _mobileGroup; break;//Test
            case "Change": _gameManager.FightIsStarted = true; break;
            case "Go": ObjectSetPosition(); break;
        }
        _battleMenu.SetActive(false);
        if (isNotNull)
        {
            towerOffset = new Vector3(_selectedObject.transform.position.x, 0.3f, _selectedObject.transform.position.z);
            if (HexagonIsBusy())
            {
                Debug.Log($"The {_selectedObject} is busy");
                return;
            }
            if (_choosedObject != null)
            {
                if (_choosedObject.GetComponent<UnitCost>().cost <= _money)
                {
                    GameObject newBuild = Instantiate(_choosedObject, towerOffset, Quaternion.identity);
                    _selectedObject.GetComponent<SixAngelSelection>().ReferenceOfObject = newBuild;
                    _money -= newBuild.GetComponent<UnitCost>().cost;
                    UpdateMoney();
                    List<SixAngelSelection> VisionCell = new List<SixAngelSelection>();
                    CellVisability CellVisability = new CellVisability();
                    SixAngelSelection newCell = _selectedObject.GetComponent<SixAngelSelection>();
                    CellVision newCellVision = new CellVision();
                    if (_builtMode == true)
                    {

                        switch (_gameManager.SelectedObject)
                        {
                            
                            case "1":
                                if (CheckSelectedObject(_airDefenses, _selectedObject)) return;

                                _airDefenses.Add(newBuild); _AllReservedCell.Add(newBuild); _airDefensesControler.Add(newBuild.GetComponent<AirDefenseController>());
                                if (_radars.Count > 0)
                                {
                                    for (int i = 0; i < _radars.Count; i++)
                                    {
                                        Debug.Log(newBuild.name + "");
                                        _radars[i].GetComponent<RadarDetection>()._airDefenses = _airDefensesControler;
                                    }
                                }

                                if (_ecss.Count > 0)
                                {
                                    for (int i = 0; i < _ecss.Count; i++)
                                    {
                                        _airDefenses[i].GetComponent<AirDefenseController>().ECS = _ecss;
                                    }
                                }
                                
                                break;
                            case "2":
                                if (CheckSelectedObject(_radars, _selectedObject)) return;
                                
                                _radars.Add(newBuild); _AllReservedCell.Add(newBuild);
                                RadarDetection newRadarDetection = newBuild.GetComponent<RadarDetection>();
                                if (_radars.Count > 0)
                                {
                                    for (int i = 0; i < _radars.Count; i++)
                                    {
                                        _radars[i].GetComponent<RadarDetection>()._airDefenses = _airDefensesControler;
                                        if (_ecss.Count > 0) _radars[i].GetComponent<RadarDetection>().ECS = _ecss;
                                        
                                    }
                                    newCellVision = newBuild.GetComponent<CellVision>();
                                    VisionCell = CellVisability.GetCellsInRange(newCell.IndexX, newCell.IndexY, newCellVision.Range, _cells.GetComponent<GridCell>()._hexagonGrid);
                                    newCellVision.getMyCells(VisionCell); 
                                }
                                break;
                            case "3":
                                if (CheckSelectedObject(_ecss, _selectedObject)) return;
                                _ecss.Add(newBuild); _AllReservedCell.Add(newBuild);
                                if (_radars.Count > 0)
                                {
                                    for (int i = 0; i < _radars.Count; i++)
                                    {
                                        _radars[i].GetComponent<RadarDetection>().ECS = _ecss;
                                    }
                                }
                                if (_airDefenses.Count > 0)
                                {
                                    for (int i = 0; i < _airDefenses.Count; i++)
                                    {
                                        _airDefenses[i].GetComponent<AirDefenseController>().ECS = _ecss;
                                    }
                                }
                                break;
                            case "TestGepard":
                            case "TestMGroup":
                                
                                VehicleCellMovement newVehicle = newBuild.GetComponent<VehicleCellMovement>();
                                newCellVision = newBuild.GetComponent<CellVision>();
                                newVehicle.VehiclePosition.Item1 = newCell.IndexX;
                                newVehicle.VehiclePosition.Item2 = newCell.IndexY;
                                newVehicle.IndexX = newCell.IndexX;
                                newVehicle.IndexY = newCell.IndexY;
                                newVehicle.Spawn();
                                VisionCell = CellVisability.GetCellsInRange(newCell.IndexX,newCell.IndexY, newCellVision.Range, _cells.GetComponent<GridCell>()._hexagonGrid);
                                newCellVision.getMyCells(VisionCell);
                                _tmpCellSelectedObject = _selectedObject;
                                _AllReservedCell.Add(newBuild);
                                _mobileGroups.Add(newBuild);
                                break;
                        }
                        _gameManager.NightLights.Add(newBuild.GetComponent<NightLight>());
                        message = " ";
                        _choosedObject = null;
                    }
                }
                else
                {
                    _selectedObject = null;
                    return;
                }
            }
        }
        _selectedObject = null;
    }
    private bool HexagonIsBusy()
    {
        GameObject ReferenceFromSelectedObject = _selectedObject.GetComponent<SixAngelSelection>().ReferenceOfObject;
        if (ReferenceFromSelectedObject != null) return true;
        else return false;
    }
    private void ObjectSetPosition()
    {
        bool isPressed = Input.touchCount > 0;
        bool isNotNull = isPressed && _selectedObject != null && _selectedObject.tag == _nameZone[0];
        bool TowerCanBuilt = _electroStations.Count > 1 && _gameManager.SelectedObject == "0";
        bool AirDeffenseCanBuilt = _airDefenses.Count > 3 && _gameManager.SelectedObject == "1";
        bool RadarCanBuilt = _radars.Count > 3 && _gameManager.SelectedObject == "2";
        bool GepardCanBuilt = _gepards.Count > 1 && _gameManager.SelectedObject == "TestGepard";
        bool MGroupCanBuilt = _mobileGroups.Count > 1 && _gameManager.SelectedObject == "TestMGroup";
        


        bool isAssigned = _gameManager.SelectedObject != null;
        Vector3 towerOffset = new Vector3(0,0,0);
        switch (_gameManager.SelectedObject)
        {
            case "Go":
                if(_selectedObject != null && _tmpSelectedObject != null && _selectedObject.tag != "DeadZone")
                {
                    
                    VehicleCellMovement tmpVehicleCellMovement = _tmpSelectedObject.GetComponent<VehicleCellMovement>();
                    SixAngelSelection tmpHexagon = _selectedObject.GetComponent<SixAngelSelection>();
                    tmpVehicleCellMovement.GetChoosedHexagon(tmpHexagon.IndexX, tmpHexagon.IndexY, _selectedObject.transform, true);
                    tmpVehicleCellMovement.IndexX = tmpHexagon.IndexX;
                    tmpVehicleCellMovement.IndexY = tmpHexagon.IndexY;
                    tmpVehicleCellMovement.VehiclePosition.Item1 = tmpHexagon.IndexX;
                    tmpVehicleCellMovement.VehiclePosition.Item2 = tmpHexagon.IndexY;
                    tmpVehicleCellMovement.transform.localScale = new Vector3(tmpVehicleCellMovement.transform.localScale.x - .1f, tmpVehicleCellMovement.transform.localScale.y - .1f, tmpVehicleCellMovement.transform.localScale.z - .1f);
                    _selectedObject.GetComponent<SixAngelSelection>().ReferenceOfObject = _tmpSelectedObject;
                    _tmpCellSelectedObject.GetComponent<SixAngelSelection>().ReferenceOfObject = null;
                    _tmpCellSelectedObject = _selectedObject;
                    _tmpSelectedObject = null;
                    _builtMode = true;
                    _selectMode = false;
                    //_subButttleMenu.active = false;
                    _battleMenu.SetActive(true);
                    
                    Destroy(_geoPointRef);
                }
                break;
        }

        
        if(_AdminBool == true)
        {
            if (isNotNull)
            {
                towerOffset = new Vector3(_selectedObject.transform.position.x, 0.3f, _selectedObject.transform.position.z);
                Touch touch = Input.GetTouch(0);
                if (touch.phase == UnityEngine.TouchPhase.Began)
                {
                    if (HexagonIsBusy())
                    {
                        Debug.Log($"The {_selectedObject} is busy");
                        return;
                    }
                    else
                    {
                        if (_tmpSelectedObject != null && _tmpSelectedObject.tag == MilitaryObject)
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
        }
        
        else return;
        if(TowerCanBuilt)
        {
            Destroy(_electroStations[0]);
            _electroStations.RemoveAt(0);
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
