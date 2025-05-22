using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsSpawner : MonoBehaviour
{
    [SerializeField] private bool IsGeneratingMap = false;
    [SerializeField] private List<GameObject> _typeOfHexagons;
    [SerializeField] private GridCell _hexagonObject;
    [SerializeField] private GameObject[,] _hexagons;
    [SerializeField] private Transform _parentContainer;
    [SerializeField] int x = 16;
    [SerializeField] int y = 16;
    [SerializeField] private float hexWidth = 1.0f;
    [SerializeField] private float hexHeight = 1.0f;
    [SerializeField] private float distanceBetweenTwoHex = .25f;
    [SerializeField] private Vector3 offSetGrid;
    [SerializeField] private List<GameObject> _electroStations;
    [SerializeField] private List<GameObject> _subElectroStationList;
    [SerializeField] private GameObject _electroStation;
    [SerializeField] private List<GameObject> _subElectroStations;
    [SerializeField] private GameManager _gameManager;
    private void Start()
    {
        StartCoroutine(WaitBeforeUnitsGenerate());
    }
    private void GenerateMap(int x, int y)
    {
        _hexagons = new GameObject[x, y];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                float xPosition = i * hexWidth * .75f;
                float yPosition = j * hexHeight + (i % 2 == 1 ? hexHeight / 2.0f : 0.0f);
                Vector3 TmpOffset = new Vector3(yPosition+ distanceBetweenTwoHex * j, 0.5f, xPosition + distanceBetweenTwoHex * i);
                GameObject newObject = Instantiate(_typeOfHexagons[Random.Range(0,2)], new Vector3(0,0,0), Quaternion.identity, _parentContainer);
                _hexagons[i, j] = newObject;
                _hexagons[i,j].transform.position = TmpOffset;
                newObject.GetComponent<SixAngelSelection>().IndexX = i;
                newObject.GetComponent<SixAngelSelection>().IndexY = j;
            }
        }
        _parentContainer.position = offSetGrid;
        //X0 = -9  Y0 = -22 
        _parentContainer.GetComponent<GridCell>().hexagonsSort();
    }
    private void SpawnElectroStation()
    {
        int x = Random.Range(4, _hexagons.GetLength(0) - 4);
        int y = Random.Range(4, _hexagons.GetLength(1) - 4);

        if (_hexagons[x, y].tag == "Zone")
        {
            GameObject NewBuild = Instantiate(_electroStation, new Vector3(_hexagons[x, y].transform.position.x, .1f, _hexagons[x, y].transform.position.z), Quaternion.identity);
            if (_electroStations.Count <= 0) _electroStations.Add(NewBuild);
            _electroStations[0].GetComponent<ElectroStation>().indexX = _hexagons[x, y].GetComponent<SixAngelSelection>().IndexX;
            _electroStations[0].GetComponent<ElectroStation>().indexY = _hexagons[x, y].GetComponent<SixAngelSelection>().IndexY;
            _gameManager.Towers.Add(NewBuild.GetComponent<Transform>());
            _hexagons[x, y].GetComponent<SixAngelSelection>().ReferenceOfObject = NewBuild;
        }
        else
        {
            x = Random.Range(0, _hexagons.GetLength(0));
            y = Random.Range(0, _hexagons.GetLength(1));
            SpawnElectroStation();
        }
    }
    private void SpawnSubElectroStation()
    {
        foreach (var SubElectroStation in _subElectroStations)
        {
        Back: int x = Random.Range(4, _hexagons.GetLength(0) - 3);
            int y = Random.Range(4, _hexagons.GetLength(1) - 3);
            if (_hexagons[x, y].tag == "Zone")
            {
                //If the Zone has one electrostation or another one things. ZOne is busy.
                if (_hexagons[x, y].GetComponent<SixAngelSelection>().ReferenceOfObject != null)
                {
                    goto Back;
                }
                GameObject NewBuild = Instantiate(SubElectroStation, new Vector3(_hexagons[x, y].transform.position.x, .1f, _hexagons[x, y].transform.position.z), Quaternion.identity);
                if (_subElectroStationList.Count <= _subElectroStations.Count) _subElectroStationList.Add(NewBuild);
                NewBuild.GetComponent<SubElectroStation>().indexX = _hexagons[x, y].GetComponent<SixAngelSelection>().IndexX;
                NewBuild.GetComponent<SubElectroStation>().indexY = _hexagons[x, y].GetComponent<SixAngelSelection>().IndexY;
                NewBuild.GetComponent<SubElectroStation>().ElectroStation = _electroStations[0];
                _gameManager.Towers.Add(NewBuild.GetComponent<Transform>());
                _hexagons[x, y].GetComponent<SixAngelSelection>().ReferenceOfObject = NewBuild;
            }
            else
            {
                goto Back;
            }
        }
    }
    IEnumerator WaitBeforeUnitsGenerate()
    {
        yield return new WaitForSeconds(0.5f);
        if (IsGeneratingMap) GenerateMap(x, y);
        else
        {
            _hexagons = _hexagonObject._hexagonGrid;
        }
        SpawnElectroStation();
        SpawnSubElectroStation();
    }

}

