using System.Collections.Generic;
using UnityEngine;

public class SixAngelSelection : MonoBehaviour
{
    public GameObject ActiveObject;
    public GameObject UnactiveObject;
    public GameObject tmpSelectedObject;
    public GameObject ReferenceOfObject;
    public int IndexX;
    public int IndexY;
    
    private void Update()
    {
        if (tmpSelectedObject == null) return;
        if(tmpSelectedObject != null && tmpSelectedObject.GetComponent<PlayerController>()._selectedObject != null && tmpSelectedObject.GetComponent<PlayerController>()._selectedObject.name != gameObject.name)
        {
            if (ReferenceOfObject != null && ReferenceOfObject.GetComponent<VehicleCellMovement>()) ReferenceOfObject.GetComponent<VehicleCellMovement>().SelectedVehicle = false;
            tmpSelectedObject = null;
            ActiveObject.SetActive(true);
            UnactiveObject.SetActive(false);
        }
    }
}
