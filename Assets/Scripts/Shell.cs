using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public LayerMask gameMask;

    private float explotionRadius;

    private BulletOwner bulletOwner;

    private float maxFlyDistance;

    private float currentDistance;

    private Direction shellDirection;

    private Vector3 Destination;

    private float maxLifeTime;

    private void Awake()
    {
        explotionRadius = HexMetric.innerRadius;
        maxLifeTime = 6f;
        maxFlyDistance = HexMetric.innerRadius * 2;
        currentDistance = 0;
    }

    private void Start()
    {
        GetDestination(transform.position.x, transform.position.y, transform.position.z);
    }
          
    private void Update()
    {
        maxLifeTime -= Time.deltaTime;

        if (maxLifeTime <= 0)
        {
           Destroy(gameObject);
        }

        GetCurrentDistance();
       
    }

    private void OnCollisionEnter()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, explotionRadius, gameMask);

        for (int i = 0; i < colliders.Length; i++)
        {

            CheckObject(colliders[i]);
            
        }

        Destroy(gameObject);

    }

    private void GetCurrentDistance()
    {
        if (shellDirection == Direction.NE_TOP_RIGHT || shellDirection == Direction.NW_TOP_LEFT)
        {
            currentDistance = Math.Abs(Vector3.Distance(transform.position, Destination));

            if (currentDistance < 1)
            {
                Destroy(gameObject);
            }

        }
        else if (shellDirection == Direction.E_RIGHT)
        {
            currentDistance = Vector3.Distance(transform.position, Destination);

            if (currentDistance < 0.5)
            {
                Destroy(gameObject);
            }
        }
        else if (shellDirection == Direction.W_LEFT)
        {
            currentDistance = Vector3.Distance(transform.position, Destination);

            if (currentDistance < 0.5)
            {
                Destroy(gameObject);
            }

        }
        else if (shellDirection == Direction.SW_LEFT_DOWN)
        {
            currentDistance = Vector3.Distance(transform.position, Destination);

            if (currentDistance < 1)
            {
                Destroy(gameObject);
            }

        }
        else if (shellDirection == Direction.SE_RIGHT_DOWN)
        {
            currentDistance = Vector3.Distance(transform.position, Destination);

            if (currentDistance < 3.5)
            {
                Destroy(gameObject);
            }

        }
    }

    private void GetDestination(float x, float y, float z)
    {
        
        Vector3 position = new Vector3 (0, 0, 0);

        switch (shellDirection)
        {
            case Direction.SE_RIGHT_DOWN:
                position.x = x + (maxFlyDistance);
                position.y = y;
                position.z = z - (maxFlyDistance * 1.577f);
                break;

            case Direction.SW_LEFT_DOWN:
                position.x = x - (maxFlyDistance);
                position.y = y;
                position.z = z - (maxFlyDistance * 1.577f);
                break;

            case Direction.W_LEFT:
                position.x = x - (maxFlyDistance) * 2;
                position.y = y;
                position.z = z;
                break;

            case Direction.E_RIGHT:
                position.x = x + (maxFlyDistance) * 2;
                position.y = y;
                position.z = z;
                break;

            case Direction.NW_TOP_LEFT:
                position.x = x - (maxFlyDistance);
                position.y = y;
                position.z = z + (maxFlyDistance * 1.577f);
                break;

            case Direction.NE_TOP_RIGHT:
                position.x = x + (maxFlyDistance);
                position.y = y;
                position.z = z + (maxFlyDistance * 1.577f);
                break;
        }

        Destination = position;

        //Debug.Log(transform.position);
        //Debug.Log(Destination);

    }

    private void CheckObject(Collider collider)
    {
        string ObjectTag = collider.tag;

        if (ObjectTag == "Player")
        {
            if (bulletOwner == BulletOwner.Player)
            {
                Destroy(gameObject);
            }
            else
            {
                collider.GetComponent<PlayerTank>().Die();
            }
        }

        if (ObjectTag == "Destructable")
        {
            if (bulletOwner == BulletOwner.Player)
            {
                collider.GetComponent<Rocks>().Ruin();
            }
        }

        if (ObjectTag == "Enemy")
        {
            if (bulletOwner == BulletOwner.Player)
            {
                collider.GetComponent<EnemyTank>().Die();
            }
        }

        if (ObjectTag == "Bonus")
        {
            if (bulletOwner == BulletOwner.Enemy)
            {
                Destroy(collider.gameObject);
            }
        }

    }

    public void SetRange(float distance)
    {
        maxFlyDistance = distance;
    }

    public void SetOwner(BulletOwner owner)
    {
        bulletOwner = owner;
    }

    public void SetDirection(Direction direction)
    {
        shellDirection = direction;
    }

}
 
public enum BulletOwner
{
    Player,
    Enemy
}