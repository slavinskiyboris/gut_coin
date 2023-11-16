using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUp : MonoBehaviour
{
    [SerializeField]
    private Button _continue;
    
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ContinueOnClick()
    {
        GameCanvas.Instance.OpenView(GameCanvas.View.Game);
    }

    private void OnEnable()
    {
        _continue.onClick.AddListener(ContinueOnClick);
    }

    private void OnDisable()
    {
        _continue.onClick.RemoveAllListeners();
    }
}
