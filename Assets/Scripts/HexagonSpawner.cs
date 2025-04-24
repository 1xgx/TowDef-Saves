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
    [SerializeField] private float hexWidth = 1.0f;
    [SerializeField] private float hexHeight = 1.0f;
    [SerializeField] private float distanceBetweenTwoHex = .25f;
    [SerializeField] private Vector3 offSetGrid;
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

}

