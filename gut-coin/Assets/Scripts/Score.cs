using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField]
    private Button _back;
    
    [SerializeField]
    private GameObject _itemPrefab;
    [SerializeField]
    private Transform _contentTr;

    private LeaderboardItem[] _items = new LeaderboardItem[0];

    public void Show()
    {
        gameObject.SetActive(true);
        
        _items = new LeaderboardItem[10];
        
        for (int i = 0; i < 10; i++)
        {
            GameObject itemGO = Instantiate(_itemPrefab, _contentTr);

            LeaderboardItem item = itemGO.GetComponent<LeaderboardItem>();

            _items[i] = item;

            int rndScore = Random.Range(1000, 5000);
            
            item.Init("Foak" + rndScore, rndScore, i + 1);
        }
    }

    public void Hide()
    {

        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i] != null)
            {
                Destroy(_items[i].gameObject);
            }
        }
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
    
    
    
    
}
