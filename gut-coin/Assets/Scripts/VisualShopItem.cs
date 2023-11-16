using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualShopItem : MonoBehaviour
{
    [SerializeField]
    private Button _buy;
    [SerializeField]
    private Button _buyRed;
    [SerializeField]
    private Button _buyLock;
    [SerializeField]
    private Text _title;
    [SerializeField]
    private Text _desc;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private Text _price;
    [SerializeField]
    private Text _level;

    private string _itemId;

    private ShopItem _shopItem;
    public void Init(ShopItem shopItem, bool exist, int playerLevel)
    {
        _shopItem = shopItem;
        
        _title.text = shopItem.Title;
        _desc.text = shopItem.Description;
        _price.text = shopItem.Price.ToString();
        _image.sprite = shopItem.Image;
        _itemId = shopItem.ItemId;
        
        RefreshStatus(exist, playerLevel);
        
        gameObject.SetActive(true);
    }

    public void RefreshStatus(bool exist, int playerLevel)
    {
        if (playerLevel < _shopItem.Level)
        {
            _buy.gameObject.SetActive(false);
            _buyRed.gameObject.SetActive(false);
            
            _buyLock.gameObject.SetActive(true);
            _level.text = $"Уровень {_shopItem.Level}";
        }
        else
        {
            if (exist)
            {
                _buy.gameObject.SetActive(false);
                _buyRed.gameObject.SetActive(true);
                _buyLock.gameObject.SetActive(false);
            }
            else
            {
                _buy.gameObject.SetActive(true);
                _buyRed.gameObject.SetActive(false);
                _buyLock.gameObject.SetActive(false);
            }   
        }
    }

    public string GetItemId()
    {
        return _itemId;
    }

    private void OnEnable()
    {
        _buy.onClick.AddListener(OnClickBuy);
    }

    private void OnDisable()
    {
        _buy.onClick.RemoveAllListeners();
    }

    private void OnClickBuy()
    {
        GameCanvas.Instance.OnClickBuy(_itemId);
    }
}
