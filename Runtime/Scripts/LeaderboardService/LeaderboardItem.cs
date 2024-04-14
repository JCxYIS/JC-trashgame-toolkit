using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LeaderboardItem : MonoBehaviour
{
    [SerializeField] Text _rankText;
    [SerializeField] Image _iconImage;
    [SerializeField] Text _nameText;
    [SerializeField] Text _scoreText;

    public void SetData(string rank, string name, string iconUrl, string score)
    {
        _rankText.text = rank;
        _nameText.text = name;
        _scoreText.text = score;  /*.ToString("N2")*/
        // TODO StartCoroutine(LoadIcon(iconUrl));
    }
}