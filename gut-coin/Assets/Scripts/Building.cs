using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public static Building Instance;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    public ShopItem CurrLevelItem;
    public int CurrLevelIndex;

    private void Awake()
    {
        Instance = this;
    }

    public void SetLevel(ShopItem currLevelItem, int currLevelIndex)
    {
        _spriteRenderer.sprite = currLevelItem.Image;
        CurrLevelIndex = currLevelIndex;
        CurrLevelItem = currLevelItem;

        PlayerPrefs.SetString(Game.BUILDING_ID, CurrLevelItem.ItemId);
    }
    
}
