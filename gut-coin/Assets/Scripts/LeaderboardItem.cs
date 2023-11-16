using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardItem : MonoBehaviour
{
    [SerializeField]
    private Text _nickname;
    [SerializeField]
    private Text _score;
    [SerializeField]
    private Text _position;

    public void Init(string nickname, int score, int position)
    {
        _nickname.text = nickname;
        _score.text = score.ToString();
        _position.text = position.ToString();
        
        gameObject.SetActive(true);
    }
}
