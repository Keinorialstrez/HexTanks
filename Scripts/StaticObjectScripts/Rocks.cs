using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocks : MonoBehaviour
{

    private Grid gameGrid;


    private void Awake()
    {
        gameGrid = FindObjectOfType<Grid>();
    }

    private void Start()
    {
        Vector3Int cellPosition = gameGrid.WorldToCell(transform.position);

        transform.position = gameGrid.GetCellCenterWorld(cellPosition);
    }

    public void Ruin()
    {
        Score.getInstance().ChangeScore(10);

        Destroy(gameObject);

        if (Random.Range(1, 7) > 3)
        {
            GetComponent<BonusSpawner>().RandomizeBonus();
        }

    }

}
