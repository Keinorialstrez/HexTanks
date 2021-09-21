using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeBonus : Bonus
{
    protected override void BonusGot()
    {
        player.GetRangeBonus();
    }

}
