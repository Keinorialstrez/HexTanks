using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteEnemyTank : EnemyTank
{
    private PlayerTank player;

    private float hexRadius;

    private float countdown;

    private int cellsReached;

    private Vector3 NextPosition;

    private Vector3 cellCenter;

    private Direction nextDirection;

    private void Awake()
    {
        player = FindObjectOfType<PlayerTank>();

        gameGrid = FindObjectOfType<Grid>();

        hexRadius = HexMetric.innerRadius;

        GetCellCenter();

        transform.position = cellCenter;

        tankCollider = GetComponent<BoxCollider>();

        ReloadTime = 2f;
        MoveSpeed = 7f;
        countdown = 2f;

        FireRange = 2 * HexMetric.innerRadius;

        currentDirection = Direction.NE_TOP_RIGHT;
    }

    private void Start()
    {
        CurrentReloadTime = ReloadTime;

        Vector3Int cellPosition = gameGrid.WorldToCell(transform.position);

        transform.position = gameGrid.GetCellCenterWorld(cellPosition);
    }

    private void Update()
    {
        CurrentReloadTime -= Time.deltaTime;

        if (CurrentReloadTime <= 0)
        {
            Fire();
        }

        countdown -= Time.deltaTime;

        if (countdown < 0f)
        {
            if (IsCellCenter() == true)
            {
                ChangeDirection();
            }
        }

        CanMove();
    }

    protected override void CanMove()
    {
        HandleSensor(currentDirection);

        Collider[] colliders = Physics.OverlapBox(tankCollider.transform.position, tankCollider.size, Quaternion.identity, gameMask);


        GetCellCenter();


        if (colliders.Length < 2)
        {
            Move();
            HandleSensor(currentDirection.Opposite());
        }
        else
        {
            HandleSensor(currentDirection.Opposite());

            bool isPlayer = false, isWall = false, isRock = false;

            for (int i = 0; i < colliders.Length; i++)
            {
                string ObjectTag = colliders[i].gameObject.tag;

               
                //HandleSensor(currentDirection);

                if (ObjectTag == "Player")
                {
                    isPlayer = true;
                }

                if (ObjectTag == "Wall" || ObjectTag == "Enemy")
                {
                    isWall = true;
                }

                if (ObjectTag == "Destructable" || ObjectTag == "Steel")
                {
                    isRock = true;
                }
            }

            if (isPlayer)
            {
                player.Die();

                currentDirection = currentDirection.Opposite();

                Move();
            }
            else if (isWall)
            {
                currentDirection = currentDirection.Opposite();

                Move();
            }
            else if (isRock)
            {
                ChangeDirectionToNext();

                Move();
            }


        }

    }

    bool IsCellCenter()
    {
        GetCellCenter();

        float currentDistance = Vector3.Distance(transform.position, cellCenter);

        if (currentDistance < 0.5)
        {
            cellsReached++;

            countdown = 1f;

            return true;

        }
        else return false;
    }

    void GetCellCenter()
    {
        Vector3Int cellPosition = gameGrid.WorldToCell(transform.position);

        cellCenter = gameGrid.GetCellCenterWorld(cellPosition);
    }

    void ChangeDirection()
    {
        if (cellsReached > 0)
        {
            transform.position = cellCenter;

            cellsReached = 0;

            GetDirrection();
        }
    }

    void ChangeDirectionToNext()
    {

        transform.position = cellCenter;

        currentDirection = currentDirection.Next();

        cellsReached = -1;
    }

    void GetDirrection()
    {
        float predictedistance;

        float currentdistance = Vector3.Distance(transform.position, player.transform.position);

        for (Direction d = Direction.NE_TOP_RIGHT; d <= Direction.NW_TOP_LEFT; d++)
        {
            PredictPosition(d, transform.position.x, transform.position.y, transform.position.z);

            predictedistance = Vector3.Distance(NextPosition, player.transform.position);

            if (predictedistance < currentdistance)
            {
                currentdistance = predictedistance;

                nextDirection = d;
            }

        }

        currentDirection = nextDirection;

    }

    private void PredictPosition(Direction direction, float x, float y, float z)
    {

        Vector3 position = new Vector3(0, 0, 0);

        switch (direction)
        {
            case Direction.SE_RIGHT_DOWN:
                position.x = x + (hexRadius);
                position.y = y;
                position.z = z - (hexRadius * 1.577f);
                break;

            case Direction.SW_LEFT_DOWN:
                position.x = x - (hexRadius);
                position.y = y;
                position.z = z - (hexRadius * 1.577f);
                break;

            case Direction.W_LEFT:
                position.x = x - (hexRadius) * 2;
                position.y = y;
                position.z = z;
                break;

            case Direction.E_RIGHT:
                position.x = x + (hexRadius) * 2;
                position.y = y;
                position.z = z;
                break;

            case Direction.NW_TOP_LEFT:
                position.x = x - (hexRadius);
                position.y = y;
                position.z = z + (hexRadius * 1.577f);
                break;

            case Direction.NE_TOP_RIGHT:
                position.x = x + (hexRadius);
                position.y = y;
                position.z = z + (hexRadius * 1.577f);
                break;
        }

        NextPosition = position;

    }

    public override void Die()
    {
        Score.getInstance().ChangeScore(1000);

        base.Die();
    }
}
