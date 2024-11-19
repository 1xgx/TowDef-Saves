using System.Collections.Generic;
using UnityEngine;

public class SixAngelSelection : MonoBehaviour
{
    public GameObject ActiveObject;
    public GameObject UnactiveObject;
    public GameObject tmpSelectedObject;
    public int IndexX;
    public int IndexY;
    private void Update()
    {
        if(tmpSelectedObject != null && tmpSelectedObject.GetComponent<PlayerController>()._selectedObject.name != gameObject.name)
        {
            tmpSelectedObject.name = null;
            ActiveObject.SetActive(true);
            UnactiveObject.SetActive(false);
        }
    }
}
