using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpawner : MonoBehaviour
{
    
    public ReloadBonus reloadPrefab;

    public RangeBonus rangePrefab;

    public void RandomizeBonus()
    {
        int BonusType;

        BonusType = Random.Range(0, 5);

        if (BonusType == 0)
        {
            Instantiate(reloadPrefab, transform.position, Quaternion.identity);
        }
        if (BonusType == 1)
        {
            Instantiate(rangePrefab, transform.position, Quaternion.identity);
        }

    }

}
