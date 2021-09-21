using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bonus : MonoBehaviour
{

    protected float countdown;

    protected PlayerTank player;

    protected Grid gameGrid;


    protected void Awake()
    {
        gameGrid = FindObjectOfType<Grid>();

        Vector3Int cellPosition = gameGrid.WorldToCell(transform.position);

        transform.position = gameGrid.GetCellCenterWorld(cellPosition);

        transform.position += new Vector3(0, 2, 0);

    }

    protected void Start()
    {
        player = FindObjectOfType<PlayerTank>();

        countdown = 10f;
    }


    protected void Update()
    {
        countdown -= Time.deltaTime;

        if (countdown < 0f)
        {
            Destroy(gameObject);
        }

        transform.Rotate(0, Time.deltaTime * 60, 0);

        CheckPlayer();

    }

    protected void CheckPlayer()
    {
        float currentDistance = Vector3.Distance(transform.position, player.transform.position);

        if (currentDistance < HexMetric.innerRadius)
        {
            BonusGot();
            Destroy(gameObject);
        }
    }

    protected abstract void BonusGot();

}
