using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _typeOfHexagons;
    [SerializeField] private GameObject[,] _hexagons;
    [SerializeField] private Transform _parentContainer;
    [SerializeField] int x = 16;
    [SerializeField] int y = 16;
    private void Awake()
    {
        GenerateMap(x, y);
    }
    private void GenerateMap(int x, int y)
    {
        _hexagons = new GameObject[x, y];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                GameObject newObject = Instantiate(_typeOfHexagons[Random.Range(0,2)], new Vector3(-9 + 1 * i,0.5f,-22 + 1 * j), Quaternion.identity, _parentContainer);
                _hexagons[i, j] = newObject;
                newObject.GetComponent<SixAngelSelection>().IndexX = i;
                newObject.GetComponent<SixAngelSelection>().IndexY = j;
            }
        }
        //X0 = -9  Y0 = -22 
        _parentContainer.GetComponent<GridCell>().hexagonsSort();
    }

}

