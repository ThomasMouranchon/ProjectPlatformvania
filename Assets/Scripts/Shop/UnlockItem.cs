using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockItem : MonoBehaviour
{ 
    public enum Item
    {
        Dash,
        Glide,
        WallJump,
        Yoyo,
        SpeedBoost,
        Bomb,
        PhantomDash
    }

    public Item targetedItem;
    public bool targetedState = true;
    private ItemsHandler itemsHandler;

    private void Start()
    {
        itemsHandler = ItemsHandler.Instance;

        switch (targetedItem)
        {
            default:
                itemsHandler.EnableDash(targetedState);
                break;
            case Item.Glide:
                itemsHandler.EnableGlide(targetedState);
                break;
            case Item.WallJump:
                itemsHandler.EnableWallJump(targetedState);
                break;
            case Item.Yoyo:
                itemsHandler.EnableYoyo(targetedState);
                break;
            case Item.SpeedBoost:
                itemsHandler.EnableSpeedBoost(targetedState);
                break;
            case Item.Bomb:
                itemsHandler.EnableBomb(targetedState);
                break;
            case Item.PhantomDash:
                itemsHandler.EnablePhantomDash(targetedState);
                break;
        }
    }
}
