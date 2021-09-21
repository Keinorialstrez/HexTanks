using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularEnemyTank : EnemyTank
{
    private float countdown;

    private int cellsReached;

    private PlayerTank player;

    void Awake()
    {

        gameGrid = FindObjectOfType<Grid>();

        tankCollider = GetComponent<BoxCollider>();

        player = FindObjectOfType<PlayerTank>();

        cellsReached = 0;

        countdown = 2f;

        ReloadTime = 2f;
        MoveSpeed = 5f;

        FireRange = 2 * HexMetric.innerRadius;

        currentDirection = Direction.SE_RIGHT_DOWN;
    }

    private void Start()
    {
        CurrentReloadTime = ReloadTime;

        Vector3Int cellPosition = gameGrid.WorldToCell(transform.position);

        transform.position = gameGrid.GetCellCenterWorld(cellPosition);
    }

    void Update()
    {
        CurrentReloadTime -= Time.deltaTime;

        if (CurrentReloadTime <= 0)
        {
            Fire();
        }

        countdown -= Time.deltaTime;

        if (countdown <= 0f)
        {

            IsCellCenter();

        }

        CanMove();
    }

    void IsCellCenter()
    {
        Vector3Int cellPosition = gameGrid.WorldToCell(transform.position);

        Vector3 cellCenter = gameGrid.GetCellCenterWorld(cellPosition);

        float currentDistance = Vector3.Distance(transform.position, cellCenter);

        if (currentDistance < 0.5)
        {
            cellsReached++;

            countdown = 2f;
        }

        if (cellsReached > 1)
        {
            transform.position = cellCenter;
            cellsReached = 0;
            RandomDirection();
        }


    }

    void RandomDirection()
    {
        int direction;

        direction = Random.Range(0, 6);

        currentDirection = (Direction)direction;

        Vector3Int cellPosition = gameGrid.WorldToCell(transform.position);

        transform.position = gameGrid.GetCellCenterWorld(cellPosition);
    }

    protected override void CanMove()
    {
        HandleSensor(currentDirection);

        Collider[] colliders = Physics.OverlapBox(tankCollider.transform.position, tankCollider.size, Quaternion.identity, gameMask);

        if (colliders.Length < 2)
        {
            Move();
            HandleSensor(currentDirection.Opposite());
        }
        else
        {
            HandleSensor(currentDirection.Opposite());

            bool isPlayer = false;

            for (int i = 0; i < colliders.Length; i++)
            {

                if (colliders[i].gameObject.tag == "Player")
                {
                    isPlayer = true;
                }

            }

            if (isPlayer == true)
            {
                player.Die();

                currentDirection = currentDirection.Opposite();

                Move();
            }
            else
            {
                currentDirection = currentDirection.Opposite();
                Move();
            }
        }

    }

    public override void Die()
    {
        Score.getInstance().ChangeScore(200);

        base.Die();
    }

}
