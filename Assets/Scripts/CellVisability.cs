using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellVisability
{
    public List<SixAngelSelection> GetCellsInRange(int centerX, int centerY, int range, GameObject[,] Cells)
    {
        List<SixAngelSelection> visibleCells = new List<SixAngelSelection>();

        for (int x = centerX - range; x <= centerX + range; x++)
        {
            for (int y = centerY - range; y <= centerY + range; y++)
            {
                // Checking to not get out in range of cell
                if (x >= 0 && x < Cells.GetLength(0) &&
                    y >= 0 && y < Cells.GetLength(1))
                {
                    // May also addjust a border of distance if we need circular radius vision.
                    int dx = Mathf.Abs(x - centerX);
                    int dy = Mathf.Abs(y - centerY);
                    if (dx + dy <= range) // Manhattan distance
                    {
                        visibleCells.Add(Cells[x, y].GetComponent<SixAngelSelection>());
                    }
                }
            }
        }

        return visibleCells;
    }
}
