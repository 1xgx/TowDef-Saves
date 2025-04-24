using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField]private List<GameObject> _hexagonsAll;
    public GameObject[,] _hexagonGrid;
    private void Awake()
    {
        hexagonsSort();
    }
    
    public void hexagonsSort()
    {
        int maxX = 0;
        int maxY = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject newGameObject = transform.GetChild(i).gameObject;
            _hexagonsAll.Add(newGameObject);
        }
        foreach (GameObject hexagon in _hexagonsAll)
        {
            if (hexagon.GetComponent<SixAngelSelection>().IndexX > maxX) maxX = hexagon.GetComponent<SixAngelSelection>().IndexX;
            if (hexagon.GetComponent<SixAngelSelection>().IndexY > maxY) maxY = hexagon.GetComponent<SixAngelSelection>().IndexY;
        }
        _hexagonGrid = new GameObject[maxX + 1, maxY + 1];
        foreach(GameObject hexagon in _hexagonsAll)
        {
            _hexagonGrid[hexagon.GetComponent<SixAngelSelection>().IndexX, hexagon.GetComponent<SixAngelSelection>().IndexY] = hexagon;
        }
        for (int i = 0; i < _hexagonsAll.Count; i++)
        {
            _hexagonsAll[i].GetComponent<SixAngelSelection>().Index = i;
        }
        for (int x = 0; x < maxX; x++)
        {
            for(int y = 0; y < maxY; y++)
            {
                if (_hexagonGrid[x,y] != null)
                {
                    Debug.Log("Missle not null !");
                }
            }
        }
    }
}
