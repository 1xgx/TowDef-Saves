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
    public int Index;
    
    private void Update()
    {
        
        if (tmpSelectedObject == null) return;
        if (tmpSelectedObject != null && tmpSelectedObject.GetComponent<PlayerController>()._selectedObject != null)
        {
            if (ReferenceOfObject != null && ReferenceOfObject.GetComponent<VehicleCellMovement>()) ReferenceOfObject.GetComponent<VehicleCellMovement>().SelectedVehicle = false;
            SixAngelSelection tmpSixAngel = tmpSelectedObject.GetComponent<PlayerController>().
            _selectedObject.GetComponent<SixAngelSelection>();
            if (tmpSixAngel && tmpSixAngel.Index != Index)
            {
                tmpSelectedObject = null;
                ActiveObject.SetActive(true);
                UnactiveObject.SetActive(false);
            }
            
        }
    }
}
