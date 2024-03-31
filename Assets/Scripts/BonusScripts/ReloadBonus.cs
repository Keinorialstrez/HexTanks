using UnityEngine;

public class ReloadBonus : Bonus
{
    protected override void BonusGot()
    {
        player.GetReloadBonus();
    }
}
