using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    public RegularEnemyTank prefabRegularEnemy;

    public EliteEnemyTank prefabEliteEnemy;

    public Rocks prefabRock;

    public SteelRock prefabSteelRock;

    public Transform rocks;

    public Transform enemys;

    private PlayerTank player;

    private HexGrid hexGrid;

    private HexCell[] cells;

    int regularEnemysMax;

    int eliteEnemysMax;

    int steelRockMax;

    int rockMax;

    private void Awake()
    {

        regularEnemysMax = 4;

        eliteEnemysMax = 1;

        steelRockMax = 4;

        rockMax = 16;
    }

    private void Start()
    {
       player = FindObjectOfType<PlayerTank>();

       hexGrid = GetComponent<HexGrid>();

       MapGenerator();
        
    }

    private void MapGenerator()
    {

        cells = hexGrid.GetHexCells();

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].GetNeighborCount();

            if (cells[i].Neighbors() > 4 && cells[i].transform.position != player.transform.position)
            {
                RandomObject(cells[i]);
            }

        }
    }

    private void RandomObject(HexCell cell)
    {
        int objectType;

        objectType = Random.Range(0, 9);

        if (objectType == (int)ObjectType.Rock && rockMax > 0)
        {
            rockMax--;
            Rocks Rock = Instantiate<Rocks>(prefabRock);
            Rock.transform.SetParent(rocks, false);
            Rock.transform.localPosition = cell.transform.position;
        }
        else if (objectType == (int)ObjectType.SteelRock && steelRockMax > 0)
        {
            steelRockMax--;
            SteelRock steelRock = Instantiate <SteelRock> (prefabSteelRock);
            steelRock.transform.SetParent(rocks, false);
            steelRock.transform.localPosition = cell.transform.position;
        }
        else if (objectType == (int)ObjectType.RegularEnemy && regularEnemysMax > 0)
        {
            regularEnemysMax--;
            RegularEnemyTank regularEnemy = Instantiate <RegularEnemyTank> (prefabRegularEnemy);
            regularEnemy.transform.SetParent(enemys, false);
            regularEnemy.transform.localPosition = cell.transform.position;
        }
        else if (objectType == (int)ObjectType.EliteEnemy && eliteEnemysMax > 0)
        {
            eliteEnemysMax--;
            EliteEnemyTank eliteEnemy = Instantiate <EliteEnemyTank> (prefabEliteEnemy);
            eliteEnemy.transform.SetParent(enemys, false);
            eliteEnemy.transform.localPosition = cell.transform.position;
        }

    }

    public enum ObjectType
    {
        Rock,
        SteelRock,
        EliteEnemy,
        RegularEnemy
    }


}
