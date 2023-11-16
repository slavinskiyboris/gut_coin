using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    public static GameCanvas Instance;
    
    [SerializeField]
    private Shop _shop;
    [SerializeField]
    private Score _score;
    [SerializeField] 
    private LevelUp _levelUp;

    public Text Gold;
    public Text Level;
    public Text LevelEXP;
    
    public Button Shop;
    public Button Touch;
    public Button Score;
    public Button SFX;
    public Button Music;
    public Button BuildingUp;
    public Text BuildingUpPrice;

    public enum View
    {
        Game,
        Shop,
        Score,
        LevelUp
    }
    
    private void Awake()
    {
        Instance = this;
    }

    private void TouchClickListener()
    {
        Game.Instance.TouchAction();
    }

    private void ShopClickListener()
    {
        OpenView(View.Shop);
    }

    private void ScoreClickListener()
    {
        OpenView(View.Score);
    }
    
    private void OnEnable()
    {
        Touch.onClick.AddListener(TouchClickListener);
        Shop.onClick.AddListener(ShopClickListener);
        Score.onClick.AddListener(ScoreClickListener);
        BuildingUp.onClick.AddListener(OnClickUpgrade);
    }

    private void OnDisable()
    {
        Touch.onClick.RemoveAllListeners();
        Shop.onClick.RemoveAllListeners();
        Score.onClick.RemoveAllListeners();
        BuildingUp.onClick.RemoveAllListeners();
    }

    public void RefreshEXP(float exp, float maxExp)
    {
        LevelEXP.text = $"{exp}/{maxExp}";
    }

    public void RefreshGold(float gold)
    {
        Gold.text = gold.ToString("F1");
    }

    public void RefreshLevel(int lvl)
    {
        Level.text = lvl.ToString();
    }

    public void OpenView(View view)
    {
        switch (view)
        {
            case View.Game:
                _shop.Hide();
                _score.Hide();
                _levelUp.Hide();
                break;
            case View.Shop:
                _score.Hide();
                _levelUp.Hide();
                _shop.Show();
                break;
            case View.Score:
                _shop.Hide();
                _levelUp.Hide();
                _score.Show();
            break;
            case View.LevelUp:
                _shop.Hide();
                _score.Hide();
                _levelUp.Show();
            break;
        }
    }

    public void OnClickBuy(string itemId)
    {
        _shop.Buy(itemId);
    }

    private void OnClickUpgrade()
    {
        Game.Instance.Upgrade();
    }

    public void ReadyToUpgrade(bool ready)
    {
        if (ready)
        {
            BuildingUp.gameObject.SetActive(true);
            BuildingUpPrice.text = Game.Instance.BuildingLevels[Building.Instance.CurrLevelIndex + 1].Price.ToString();
        }
        else
        {
            BuildingUp.gameObject.SetActive(false);
        }
    }

    public Shop GetShop()
    {
        return _shop;
    }
}
