using UnityEngine;

public abstract class EnemyTank : MonoBehaviour, IMoveble
{
    public float MoveSpeed { get; set; }

    protected float ReloadTime;

    protected float CurrentReloadTime;

    protected float FireRange;

    protected Direction currentDirection;

    protected BoxCollider tankCollider;

    protected Grid gameGrid;

    public Shell shellPrefab;

    public Transform fireTransform;

    public LayerMask gameMask;

    public void Move()
    {

        switch (currentDirection)
        {
            case Direction.SE_RIGHT_DOWN:
                transform.position += new Vector3(MoveSpeed * 0.866025404f * Time.deltaTime, 0, -MoveSpeed * 1.5f * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 160, 0);
                break;
            case Direction.SW_LEFT_DOWN:
                transform.position += new Vector3(-MoveSpeed * 0.866025404f * Time.deltaTime, 0, -MoveSpeed * 1.5f * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 210, 0);
                break;
            case Direction.W_LEFT:
                transform.position += new Vector3(-MoveSpeed * 0.866025404f * Time.deltaTime * 1.5f, 0, 0);
                transform.rotation = Quaternion.Euler(0, 270, 0);
                break;
            case Direction.E_RIGHT:
                transform.position += new Vector3(MoveSpeed * 0.866025404f * Time.deltaTime * 1.5f, 0, 0);
                transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case Direction.NW_TOP_LEFT:
                transform.position += new Vector3(-MoveSpeed * 0.866025404f * Time.deltaTime, 0, MoveSpeed * 1.5f * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 330, 0);
                break;
            case Direction.NE_TOP_RIGHT:
                transform.position += new Vector3(MoveSpeed * 0.866025404f * Time.deltaTime, 0, MoveSpeed * 1.5f * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 30, 0);
                break;
        }
    }

    protected virtual void CanMove()
    {
        HandleSensor(currentDirection);

        Collider[] colliders = Physics.OverlapBox(tankCollider.transform.position, tankCollider.size, Quaternion.identity, gameMask);

        if (colliders.Length < 2)
        {
            Move();
        }

        HandleSensor(currentDirection.Opposite());

    }


    protected void HandleSensor(Direction direction)
    {
        switch (direction)
        {
            case Direction.SE_RIGHT_DOWN:
                tankCollider.transform.position += new Vector3(MoveSpeed * 0.866025404f * Time.deltaTime, 0, -MoveSpeed * 1.5f * Time.deltaTime);
                break;
            case Direction.SW_LEFT_DOWN:
                tankCollider.transform.position += new Vector3(-MoveSpeed * 0.866025404f * Time.deltaTime, 0, -MoveSpeed * 1.5f * Time.deltaTime);
                break;
            case Direction.W_LEFT:
                tankCollider.transform.position += new Vector3(-MoveSpeed * 0.866025404f * Time.deltaTime * 2f, 0, 0);
                break;
            case Direction.E_RIGHT:
                tankCollider.transform.position += new Vector3(MoveSpeed * 0.866025404f * Time.deltaTime * 2f, 0, 0);
                break;
            case Direction.NW_TOP_LEFT:
                tankCollider.transform.position += new Vector3(-MoveSpeed * 0.866025404f * Time.deltaTime, 0, MoveSpeed * 1.5f * Time.deltaTime);
                break;
            case Direction.NE_TOP_RIGHT:
                tankCollider.transform.position += new Vector3(MoveSpeed * 0.866025404f * Time.deltaTime, 0, MoveSpeed * 1.5f * Time.deltaTime);
                break;
        }
    }

    protected void Fire()
    {

        Shell shell = Instantiate(shellPrefab, fireTransform.position, fireTransform.rotation);

        shell.SetOwner(BulletOwner.Enemy);

        shell.SetRange(FireRange);

        shell.SetDirection(currentDirection);

        shell.transform.GetComponent<Rigidbody>().velocity = FireRange * fireTransform.forward * 2.5f;

        CurrentReloadTime = ReloadTime;

    }

    public virtual void Die()
    {
        Destroy(gameObject);

        FindObjectOfType<GameManager>().UpdateEnemys();

        GetComponent<BonusSpawner>().RandomizeBonus();

    }

}
