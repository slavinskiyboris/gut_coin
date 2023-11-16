using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField]
    private Button _back;

    [SerializeField]
    private GameObject _itemPrefab;
    
    [SerializeField]
    private Transform _contentTr;

    private ShopItem[] _shopItems;
    private VisualShopItem[] _visualShopItems = new VisualShopItem[0];

    public void Init()
    {
        if (_shopItems == null)
        {
            _shopItems = Resources.Load<ShopScriptableObject>("ScriptableObjects/ShopItems").Items;   
        }
    }

    public ShopItem FindShopItem(string itemId)
    {
        for (int i = 0; i < _shopItems.Length; i++)
        {
            if (_shopItems[i].ItemId == itemId)
            {
                return _shopItems[i];
            }   
        }

        return null;
    }

    public void Show()
    {
        ClearItems();
        
        gameObject.SetActive(true);

        _visualShopItems = new VisualShopItem[_shopItems.Length];
        
        for (int i = 0; i < _shopItems.Length; i++)
        {
            ShopItem shopItem = _shopItems[i];
            
            var tmpItem = Instantiate(_itemPrefab, _contentTr);

            VisualShopItem visualShopItem = tmpItem.GetComponent<VisualShopItem>();
            
            visualShopItem.Init(shopItem, Game.Instance.HaveItem(shopItem.ItemId), Game.Instance.Level);

            _visualShopItems[i] = visualShopItem;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    private void BackOnClick()
    {
        GameCanvas.Instance.OpenView(GameCanvas.View.Game);
    }

    private void OnEnable()
    {
        _back.onClick.AddListener(BackOnClick);
    }

    private void OnDisable()
    {
        _back.onClick.RemoveAllListeners();
    }

    public void Buy(string itemId)
    {
        ShopItem shopItem = FindShopItem(itemId);
        if (Game.Instance.GetGold() >= (float)shopItem.Price)
        {
            if (!Game.Instance.HaveItem(itemId))
            {
                Game.Instance.AddItemToInventory(shopItem);
                RefreshItems();
                Game.Instance.RecalcBoosts();
                
                Game.Instance.ChangeGold(-shopItem.Price);
                GameCanvas.Instance.RefreshGold(Game.Instance.GetGold());
                
                Game.Instance.SaveInventory();
            }
        }
    }

    private void ClearItems()
    {
        for (int i = 0; i < _visualShopItems.Length; i++)
        {
            if (_visualShopItems[i] != null)
            {
                Destroy(_visualShopItems[i].gameObject);   
            }
        }
    }

    private void RefreshItems()
    {
        for (int i = 0; i < _visualShopItems.Length; i++)
        {
            string itemId = _visualShopItems[i].GetItemId();
            bool exist = Game.Instance.HaveItem(itemId);
            _visualShopItems[i].RefreshStatus(exist, Game.Instance.Level);
        }
    }
}
