using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }

    [SerializeField] private Transform gridSystemVisualSinglePrefab;

    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There is more then one character selected" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        gridSystemVisualSingleArray = new GridSystemVisualSingle[LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetHeight()];
        for(int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for(int z =0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPostion gridPostion = new GridPostion(x,z);
                Transform gridSystemVisualSingleTransform = Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPostion(gridPostion), Quaternion.identity);

                gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }
    }

    private void Update()
    {
        UpdatGridVisual();
    }

    public void HideAllGridPostion()//hides grid location that are not currently selectable
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {

                gridSystemVisualSingleArray[x, z].Hide(); 
            }
        }
    }

    public void ShowGridPostionList(List<GridPostion> gridPostionsList)//shows grid postion that avaible for the player to choose from.
    {
        foreach(GridPostion gridPostion in gridPostionsList)
        {
            gridSystemVisualSingleArray[gridPostion.x, gridPostion.z].Show();
        }
       
    }

    private void UpdatGridVisual()
    {
        HideAllGridPostion();

        BaseAction selectedAction = CharacterActionSystem.Instance.GetSelectedAction();
        ShowGridPostionList (selectedAction.GetValidActionGridPostionList());
    }
}
