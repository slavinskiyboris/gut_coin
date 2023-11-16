using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopItem
{
    public string ItemId; // Айди 
    public string Title; // Название 
    public string Description; // Описание 
    public int Price; // Цена
    public Sprite Image; // Картинка
    public float Amount; // Кол-во добавлений
    public int Level; // Требуемый уровень
}
