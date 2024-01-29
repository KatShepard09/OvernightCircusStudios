using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem 
{
    private int width;
    private int height;
    private float cellSize;
    private GridObject[,] gridObjectArray;
   public GridSystem(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridObjectArray = new GridObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {

                GridPostion gridPostion = new GridPostion(x,z);
               gridObjectArray[x,z] = new GridObject(this, gridPostion);
            }
        }

        this.cellSize = cellSize;
    }

    public Vector3 GetWorldPostion(GridPostion gridPostion)
    {
        return new Vector3(gridPostion.x, 0, gridPostion.z) * cellSize;
    }

    public GridPostion GetGridPostion(Vector3 worldPosition)

    {
        return new GridPostion(Mathf.RoundToInt(worldPosition.x / cellSize),
        Mathf.RoundToInt(worldPosition.z / cellSize));
       
    }
      public void CreateDebugObjects(Transform debugPrefab) 
      {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPostion gridPostion = new GridPostion(x,z);
                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPostion(gridPostion), Quaternion.identity);
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPostion));
            }
        }
      }

       public GridObject GetGridObject(GridPostion gridPostion)
       {
         return gridObjectArray[gridPostion.x, gridPostion.z];
       }

    public bool IsValidGridPostion(GridPostion gridPostion)
    {
        return gridPostion.x >= 0 && gridPostion.z >= 0 && gridPostion.x < width && gridPostion.z < height;
    }
}
