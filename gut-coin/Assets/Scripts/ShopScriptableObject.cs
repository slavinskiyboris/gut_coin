using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItems", menuName = "Create Shop Items")]
public class ShopScriptableObject : ScriptableObject
{
    [SerializeField]
    public ShopItem[] Items = new ShopItem[0];
}
