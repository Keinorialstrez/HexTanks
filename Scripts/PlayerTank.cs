using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTank : MonoBehaviour, IMoveble
{
    public float MoveSpeed { get; set; }

    private int PlayerLives;

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

    public Joystick joystick;

    public Transform fireTransform;

    void Awake()
    {
        gameGrid = FindObjectOfType<Grid>();
        playerCollider = GetComponent<BoxCollider>();
        joystick = FindObjectOfType<FixedJoystick>();

        ReloadTime = 2f;
        MoveSpeed = 10f;
        PlayerLives = 2;

        FireRange = 2 * HexMetric.innerRadius;

        currentDirection = Direction.E_RIGHT;

        GetCellCenter();

        CenterCooldown = 0.5f;

        transform.position = cellCenter;

    }

    void Start()
    {
        LivesText.GetInstance().ChangeLivesAmount(PlayerLives);
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
        if (joystick.Horizontal >= 0.2f)
        {
            movement.x = 1;
        }
        else if (joystick.Horizontal <= -0.2f)
        {
            movement.x = -1;
        }
        else
        {
            movement.x = 0;
        }

        if (joystick.Vertical >= 0.2f)
        {
            movement.z = 1;
        }
        else if (joystick.Vertical <= -0.2f)
        {
            movement.z = -1;
        }
        else
        {
            movement.z = 0;
        }

        //movement.x = Input.GetAxisRaw("Horizontal");
        //movement.z = Input.GetAxisRaw("Vertical");


        //if (Input.GetAxis("Fire1") == 1)
        //{
        //    if (CurrentReloadTime <= 0)
        //        Fire();
        //}
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


    public void Fire()
    {
        if (CurrentReloadTime <= 0)
        {
            Shell shell = Instantiate(shellPrefab, fireTransform.position, fireTransform.rotation);

            shell.SetOwner(BulletOwner.Player);

            shell.SetRange(FireRange);

            shell.SetDirection(currentDirection);

            shell.transform.GetComponent<Rigidbody>().velocity = 2.5f * FireRange * fireTransform.forward;

            CurrentReloadTime = ReloadTime;
        }

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

        if (currentDistance < 1.5f)
        {

            return true;

        }
        else return false;
    }
    private void TryMove()
    {
        if (movement.z == -1 && movement.x == 1)
        {
            TrySet(Direction.SE_RIGHT_DOWN);

            transform.rotation = Quaternion.Euler(0, 160, 0);
        }
        else if (movement.z == -1 && movement.x == -1)
        {
            TrySet(Direction.SW_LEFT_DOWN);

            transform.rotation = Quaternion.Euler(0, 210, 0);

        }
        else if (movement.z == 1 && movement.x == 1)
        {
            TrySet(Direction.NE_TOP_RIGHT);

            transform.rotation = Quaternion.Euler(0, 30, 0);

        }
        else if (movement.z == 1 && movement.x == -1)
        {
            TrySet(Direction.NW_TOP_LEFT);

            transform.rotation = Quaternion.Euler(0, 330, 0);

        }
        else if (movement.x == -1)
        {

            TrySet(Direction.W_LEFT);

            transform.rotation = Quaternion.Euler(0, 270, 0);

        }
        else if (movement.x == 1)
        {
            TrySet(Direction.E_RIGHT);

            transform.rotation = Quaternion.Euler(0, 90, 0);

        }

        void TrySet(Direction direction)
        {

            float distance = Vector3.Distance(transform.position, cellCenter);

            bool isCellCenter = IsCellCenter();

            if (isCellCenter)
            {
                currentDirection = direction;

                if (CenterCooldown < 0f && distance > 0.1 && distance < HexMetric.innerRadius)
                {
                    GetCellCenter();
                    transform.position = cellCenter;
                    CenterCooldown = 0.25f;
                }

                CanMove();

            }
            else if (direction.IsOpposite(currentDirection) || direction == currentDirection)
            {

                currentDirection = direction;

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

        Debug.Log("You killed");

        

        this.enabled = false;

        if (PlayerLives > 0)
        {
            StartCoroutine("Ressurect", new WaitForSeconds(0.2f));
        }
        else
        {
            playerCollider.enabled = false;

            this.enabled = false;
            FindObjectOfType<GameManager>().EndGame();
            FindObjectOfType<CameraContoler>().enabled = false;
            Destroy(gameObject);
        }

    }

    private void Ressurect()
    {
        this.enabled = true;

        GetCellCenter();

        transform.position = cellCenter;

        PlayerLives--;

        LivesText.GetInstance().ChangeLivesAmount(PlayerLives);

    }
}
