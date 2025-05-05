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
        //toShowArtem();
    }
    private void FixedUpdate()
    {
        
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
                if (gameObject.GetComponent<VehicleCellMovement>()) gameObject.GetComponent<VehicleCellMovement>().Target = cell.TmpTrigger;
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
                else return;
            }
        }
    }
    private void toShowArtem()
    {
        for (int i = 0; i < VisionCells.Count; i++)
        {
            Transform newTransform = VisionCells[i].transform;
            newTransform.position = new Vector3(newTransform.position.x, 1.0f, newTransform.position.z);
        }
    }
}
