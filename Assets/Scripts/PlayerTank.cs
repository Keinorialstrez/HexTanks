using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTank : MonoBehaviour, IMoveble
{
    public float MoveSpeed { get; set; }

    private float ReloadTime;

    private float CurrentReloadTime;

    private float FireRange;

    private Vector3 movement;

    private Vector3 cellCenter;

    private Direction currentDirection;

    private BoxCollider playerCollider;

    private float CenterCooldown;

    private Grid gameGrid;

    public Shell shellPrefab;

    public LayerMask gameMask;

    public Transform fireTransform;  

    void Awake()
    {
        gameGrid = FindObjectOfType<Grid>();
        playerCollider = GetComponent<BoxCollider>();

        ReloadTime = 2f;
        MoveSpeed = 10f;

        FireRange = 2 * HexMetric.innerRadius;

        currentDirection = Direction.E_RIGHT;

        GetCellCenter();

        CenterCooldown = 0.5f;

        transform.position = cellCenter;

    }

    void Update()
    {
        CurrentReloadTime -= Time.deltaTime;

        CenterCooldown = 1f;

        Inputs();

        TryMove();
    }

    private void Inputs()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");


        if (Input.GetAxis("Fire1") == 1)
        {
            if (CurrentReloadTime <= 0)
                Fire();
        }
    }
    private void HandleSensor(Direction direction)
    {
        switch (direction)
        {
            case Direction.SE_RIGHT_DOWN:
                playerCollider.transform.position += new Vector3(MoveSpeed * 0.866025404f * Time.deltaTime, 0, -MoveSpeed * 1.5f * Time.deltaTime);
                break;
            case Direction.SW_LEFT_DOWN:
                playerCollider.transform.position += new Vector3(-MoveSpeed * 0.866025404f * Time.deltaTime, 0, -MoveSpeed * 1.5f * Time.deltaTime);
                break;
            case Direction.W_LEFT:
                playerCollider.transform.position += new Vector3(-MoveSpeed * 0.866025404f * Time.deltaTime * 2f, 0, 0);
                break;
            case Direction.E_RIGHT:
                playerCollider.transform.position += new Vector3(MoveSpeed * 0.866025404f * Time.deltaTime * 2f, 0, 0);
                break;
            case Direction.NW_TOP_LEFT:
                playerCollider.transform.position += new Vector3(-MoveSpeed * 0.866025404f * Time.deltaTime, 0, MoveSpeed * 1.5f * Time.deltaTime);
                break;
            case Direction.NE_TOP_RIGHT:
                playerCollider.transform.position += new Vector3(MoveSpeed * 0.866025404f * Time.deltaTime, 0, MoveSpeed * 1.5f * Time.deltaTime);
                break;
        }
    }

    public void Move()
    {

        switch (currentDirection)
        {
            case Direction.SE_RIGHT_DOWN:
                transform.position += new Vector3(MoveSpeed * 0.866025404f * Time.deltaTime, 0, -MoveSpeed * 1.5f * Time.deltaTime);
                break;
            case Direction.SW_LEFT_DOWN:
                transform.position += new Vector3(-MoveSpeed * 0.866025404f * Time.deltaTime, 0, -MoveSpeed * 1.5f * Time.deltaTime);
                break;
            case Direction.W_LEFT:
                transform.position += new Vector3(-MoveSpeed * 0.866025404f * Time.deltaTime * 2f, 0, 0);
                break;
            case Direction.E_RIGHT:
                transform.position += new Vector3(MoveSpeed * 0.866025404f * Time.deltaTime * 2f, 0, 0);
                break;
            case Direction.NW_TOP_LEFT:
                transform.position += new Vector3(-MoveSpeed * 0.866025404f * Time.deltaTime, 0, MoveSpeed * 1.5f * Time.deltaTime);
                break;
            case Direction.NE_TOP_RIGHT:
                transform.position += new Vector3(MoveSpeed * 0.866025404f * Time.deltaTime, 0, MoveSpeed * 1.5f * Time.deltaTime);
                break;
        }
    }


    private void Fire()
    {

        Shell shell = Instantiate(shellPrefab, fireTransform.position, fireTransform.rotation);

        shell.SetOwner(BulletOwner.Player);

        shell.SetRange(FireRange);

        shell.SetDirection(currentDirection);

        shell.transform.GetComponent<Rigidbody>().velocity = FireRange * fireTransform.forward * 2.5f; 

        CurrentReloadTime = ReloadTime;
         
    }

    void GetCellCenter()
    {
        Vector3Int cellPosition = gameGrid.WorldToCell(transform.position);

        cellCenter = gameGrid.GetCellCenterWorld(cellPosition);
    }

    bool IsCellCenter()
    {
        GetCellCenter();

        float currentDistance = Vector3.Distance(transform.position, cellCenter);

        if (currentDistance < 1.5)
        {

            return true;

        }
        else return false;
    }
    private void TryMove()
    {
        if (movement.z == -1 && movement.x == 1)
        {
            if (IsCellCenter())
            {
                currentDirection = Direction.SE_RIGHT_DOWN;

                transform.rotation = Quaternion.Euler(0, 160, 0);

                if (CenterCooldown < 0f)
                {
                    transform.position = cellCenter;
                    CenterCooldown = 0.25f;
                }

                CanMove();

            }
            else if (Direction.SE_RIGHT_DOWN.IsOpposite(currentDirection) || Direction.SE_RIGHT_DOWN == currentDirection)
            {

                currentDirection = Direction.SE_RIGHT_DOWN;

                transform.rotation = Quaternion.Euler(0, 160, 0);

                CanMove();

            }
        }
        else if (movement.z == -1 && movement.x == -1)
        {
            if (IsCellCenter())
            {
                currentDirection = Direction.SW_LEFT_DOWN;

                transform.rotation = Quaternion.Euler(0, 210, 0);

                if (CenterCooldown < 0f)
                {
                    transform.position = cellCenter;
                    CenterCooldown = 0.25f;
                }

                CanMove();
            }
            else if (Direction.SW_LEFT_DOWN.IsOpposite(currentDirection) || Direction.SW_LEFT_DOWN == currentDirection)
            {
                currentDirection = Direction.SW_LEFT_DOWN;

                transform.rotation = Quaternion.Euler(0, 210, 0);

                CanMove();
            }

        }
        else if (movement.z == 1 && movement.x == 1)
        {
            if (IsCellCenter())
            {
                currentDirection = Direction.NE_TOP_RIGHT;

                transform.rotation = Quaternion.Euler(0, 30, 0);

                if (CenterCooldown < 0f)
                {
                    transform.position = cellCenter;
                    CenterCooldown = 0.25f;
                }

                CanMove();
            }
            else if (Direction.NE_TOP_RIGHT.IsOpposite(currentDirection) || Direction.NE_TOP_RIGHT == currentDirection)
            {
                currentDirection = Direction.NE_TOP_RIGHT;

                transform.rotation = Quaternion.Euler(0, 30, 0);

                CanMove();
            }

        }
        else if (movement.z == 1 && movement.x == -1)
        {
            if (IsCellCenter())
            {
                currentDirection = Direction.NW_TOP_LEFT;

                transform.rotation = Quaternion.Euler(0, 330, 0);

                if (CenterCooldown < 0f)
                {
                    transform.position = cellCenter;
                    CenterCooldown = 0.25f;
                }

                CanMove();
            }
            else if (Direction.NW_TOP_LEFT.IsOpposite(currentDirection) || Direction.NW_TOP_LEFT == currentDirection)
            {
                currentDirection = Direction.NW_TOP_LEFT;

                transform.rotation = Quaternion.Euler(0, 330, 0);

                CanMove();
            }

        }
        else if (movement.x == -1)
        {
            if (IsCellCenter())
            {
                currentDirection = Direction.W_LEFT;

                transform.rotation = Quaternion.Euler(0, 270, 0);

                if (CenterCooldown < 0f)
                {
                    transform.position = cellCenter;
                    CenterCooldown = 0.25f;
                }

                CanMove();
            }
            else if (Direction.W_LEFT.IsOpposite(currentDirection) || Direction.W_LEFT == currentDirection)
            {
                currentDirection = Direction.W_LEFT;

                transform.rotation = Quaternion.Euler(0, 270, 0);

                CanMove();
            }

        }
        else if (movement.x == 1)
        {
            if (IsCellCenter())
            {
                currentDirection = Direction.E_RIGHT;

                transform.rotation = Quaternion.Euler(0, 90, 0);

                if (CenterCooldown < 0f)
                {
                    transform.position = cellCenter;
                    CenterCooldown = 0.25f;
                }

                CanMove();
            }
            else if (Direction.E_RIGHT.IsOpposite(currentDirection) || Direction.E_RIGHT == currentDirection)
            {
                currentDirection = Direction.E_RIGHT;

                transform.rotation = Quaternion.Euler(0, 90, 0);

                CanMove();
            }

        }

        void CanMove()
        {
            HandleSensor(currentDirection);

            Collider[] colliders = Physics.OverlapBox(playerCollider.transform.position, playerCollider.size, Quaternion.identity, gameMask);

            if (colliders.Length < 2)
            {
                Move();
            }

            HandleSensor(currentDirection.Opposite());

        }
    }

    public void GetReloadBonus()
    {
        if (ReloadTime > 1)
        {
            ReloadTime -= 1;
        }
    }

    public void GetRangeBonus()
    {
        FireRange += HexMetric.innerRadius;
    }

    public void Die()
    {
        this.enabled = false;

        Destroy(gameObject);

        FindObjectOfType<CameraContoler>().enabled = false;

        FindObjectOfType<GameManager>().EndGame();
        
    }

    

}
