using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SixAngelSelection : MonoBehaviour
{
    public GameObject ActiveObject;
    public GameObject UnactiveObject;
    public GameObject tmpSelectedObject;
    public GameObject ReferenceOfObject;
    public int IndexX;
    public int IndexY;
    public int Index;
    public Transform TmpTrigger;
    
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Missle")
        {
            TmpTrigger = other.GetComponent<Transform>();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Missle")
        {
            TmpTrigger = null;
        }
    }
}
