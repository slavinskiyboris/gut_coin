using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SimpleJSON;

public class Game : MonoBehaviour
{
    public const string EXIST_SAVE = "EXISTSAVE";
    public const string GOLD = "GOLD";
    public const string EXP = "EXP";
    public const string LEVEL = "LEVEL";
    public const string BUILDING_ID = "BUILDINGID";
    public const string INVENTORY = "INVENTORY";
    public const string MUSIC = "MUSIC";
    
    public static Game Instance;

    private int _level;
    public int Level 
    {
        get { return _level; }
    } 
    private float _goldBoost;
    private float _gold;
    
    private int _expBoost;
    private int _maxExp;
    private int _exp;

    private TimeTrigger _timeTrigger;

    public bool ReadyToUpgrade;

    public ShopItem[] BuildingLevels;

    private Dictionary<string, ShopItem> _existItems = new Dictionary<string, ShopItem>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Init();
        Debug.Log("Init Done!");
    }

    public void TouchAction()
    {
        if (_timeTrigger.CheckAndRestart())
        {
            if (ReadyToUpgrade == false)
            {
                ChangeExp(_exp + _expBoost);
                if (_exp >= _maxExp)
                {
                    LevelUp();
                }

                GameCanvas.Instance.RefreshEXP(_exp, _maxExp);
            }

            ChangeGold(_goldBoost);
            GameCanvas.Instance.RefreshGold(_gold);
        }
    }

    private void LevelUp()
    {
        ChangeExp(0);
        ChangeLevel(_level + 1);

        RecalcMaxExp();
        
        GameCanvas.Instance.OpenView(GameCanvas.View.LevelUp);
        GameCanvas.Instance.RefreshLevel(_level);

        CalcReadyToUpgrade();
    }

    public void RecalcMaxExp()
    {
        _maxExp = _level * 100;
    }
    
    public void Init()
    {
        _timeTrigger = new TimeTrigger(0.15f);
        
        BuildingLevels = Resources.Load<ShopScriptableObject>("ScriptableObjects/BuildingsLevel").Items;
        GameCanvas.Instance.GetShop().Init();
        
        if (PlayerPrefs.HasKey(EXIST_SAVE))
        {
            // Если сохранения уже есть.
            _gold = PlayerPrefs.GetFloat(GOLD);
            _exp = PlayerPrefs.GetInt(EXP);
            _level = PlayerPrefs.GetInt(LEVEL);
            _goldBoost = 1f;
            
            string buildingId = PlayerPrefs.GetString(BUILDING_ID);

            for (int i = 0; i < BuildingLevels.Length; i++)
            {
                if (BuildingLevels[i].ItemId == buildingId)
                {
                    Building.Instance.SetLevel(BuildingLevels[i], i);
                }
            }
            
            CalcReadyToUpgrade();
            
            if (PlayerPrefs.HasKey(INVENTORY))
            {
                var js = PlayerPrefs.GetString(INVENTORY);
                JSONArray arr = JSONNode.Parse(js).AsArray;

                for (int i = 0; i < arr.Count; i++)
                {
                    _existItems.Add(arr[i], GameCanvas.Instance.GetShop().FindShopItem(arr[i]));
                }
            }

            bool music = PlayerPrefs.GetInt(MUSIC) > 0 ? true : false;
            AudioSystem.Instance.Init(music);
        }
        else
        {
            // Первая инициализация.
            _gold = 0f;
            _exp = 0;
            _level = 1;
            _goldBoost = 1f;
            Building.Instance.SetLevel(BuildingLevels[0], 0);
            GameCanvas.Instance.ReadyToUpgrade(false);
            
            PlayerPrefs.SetFloat(GOLD, _gold);
            PlayerPrefs.SetInt(EXP, _exp);
            PlayerPrefs.SetInt(LEVEL, _level);
            PlayerPrefs.SetInt(EXIST_SAVE, 1);
            PlayerPrefs.SetInt(MUSIC, 1);
            
            AudioSystem.Instance.Init(true);
        }
        
        RecalcMaxExp();
        
        RecalcExpBoost();
        
        // Визуальная часть.
        GameCanvas.Instance.RefreshEXP(_exp, _maxExp);
        GameCanvas.Instance.RefreshGold(_gold);        
        GameCanvas.Instance.RefreshLevel(_level);
    }

    public void RecalcExpBoost()
    {
        _expBoost = (int)Building.Instance.CurrLevelItem.Amount;
    }

    public void Upgrade()
    {
        int index = Building.Instance.CurrLevelIndex +1;
        ShopItem item = BuildingLevels[index];

        if (_gold >= item.Price)
        {
            Building.Instance.SetLevel(item, index);
            
            GameCanvas.Instance.ReadyToUpgrade(false);
            ReadyToUpgrade = false;
            ChangeGold(-item.Price);
            GameCanvas.Instance.RefreshGold(_gold);
            RecalcExpBoost();
        }
    }

    public void AddItemToInventory(ShopItem shopItem)
    {
        _existItems.Add(shopItem.ItemId, shopItem);
    }
    public bool HaveItem(string itemId)
    {
        return _existItems.ContainsKey(itemId);
    }

    public void RecalcBoosts()
    {
        _goldBoost = 1f;
        var items = _existItems.Values.ToArray();
        for (int i = 0; i < items.Length; i++)
        {
            if (i == 0)
            {
                _goldBoost *= items[i].Amount;
            }
            else
            {
                _goldBoost += items[i].Amount;   
            }
        }
    }

    public float GetGold()
    {
        return _gold;
    }

    public void ChangeGold(float amount)
    {
        _gold += amount;
        PlayerPrefs.SetFloat(GOLD, _gold);
    }

    public void ChangeExp(int value)
    {
        _exp = value;
        PlayerPrefs.SetInt(EXP, _exp);
    }
    
    public void ChangeLevel(int value)
    {
        _level = value;
        PlayerPrefs.SetInt(LEVEL, _level);
    }

    public void CalcReadyToUpgrade()
    {
        ReadyToUpgrade = false;
        GameCanvas.Instance.ReadyToUpgrade(false);
        
        for (int i = 0; i < BuildingLevels.Length; i++)
        {
            if (_level == BuildingLevels[i].Level)
            {
                if (Building.Instance.CurrLevelItem.ItemId != BuildingLevels[i].ItemId)
                {
                    ReadyToUpgrade = true;
                    GameCanvas.Instance.ReadyToUpgrade(true);
                }
            }

        }
    }

    public void SaveInventory()
    {
        ShopItem[] items = _existItems.Values.ToArray();
        JSONArray jsItems = new JSONArray();
        
        for (int i = 0; i < items.Length; i++)
        {
            jsItems[i] = items[i].ItemId;
        }

        string str = jsItems.ToString();
        PlayerPrefs.SetString(INVENTORY, str);
    }
}
