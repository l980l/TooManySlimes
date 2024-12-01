using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : CommonItem
{
    protected override void Use(GameObject Player)
    {
        // ÄÚÀÎ È¹µæ
        CoinManager.Instance.CoinCount++;
        gameObject.SetActive(false);
     
        SoundManager.Instance.PlaySFX(SFX.coin, transform.position);
    }
}
