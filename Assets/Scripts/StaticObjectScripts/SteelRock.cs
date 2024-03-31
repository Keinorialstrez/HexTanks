using UnityEngine;

public class SteelRock : MonoBehaviour
{

    private Grid gameGrid;

    private void Awake()
    {
        gameGrid = FindObjectOfType<Grid>();

        Vector3Int cellPosition = gameGrid.WorldToCell(transform.position);

        transform.position = gameGrid.GetCellCenterWorld(cellPosition);
    }

    public void Melt()
    {
        Score.getInstance().ChangeScore(100000000);
        Destroy(gameObject);
    }

}
