using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellVision : MonoBehaviour
{
    public int Range = 1;
    [SerializeField] private List<SixAngelSelection> VisionCells;
    public void getMyCells(List<SixAngelSelection> myCells)
    {
        VisionCells = myCells;
    }
    public void ClearMyCells() 
    {
        VisionCells = null;
    }
    private void Update()
    {
        isDetected();
    }
    private void isDetected()
    {
        foreach (var cell in VisionCells)
        {
            if (cell.TmpTrigger)
            {
                Debug.Log($"Detected {cell}");
                if (gameObject.GetComponent<VehicleCellMovement>()) gameObject.GetComponent<VehicleCellMovement>().Shooting(cell.TmpTrigger);
                if (gameObject.GetComponent<RadarDetection>())
                {
                    
                    RadarDetection newRadar = gameObject.GetComponent<RadarDetection>();
                   
                    newRadar.getDetectedObject(cell.TmpTrigger);
                }
            }
            if (gameObject.GetComponent<RadarDetection>())
            {
                RadarDetection newRadar = gameObject.GetComponent<RadarDetection>();
                if (cell.ReferenceOfObject != null && cell.ReferenceOfObject.tag == "ECS")
                {
                    Debug.Log("hsdbfhjsdbhjfbshjdbfhjsdbhjbfhs");
                    newRadar.ECSIsPosed = true;
                }
            }
        }
    }
}
