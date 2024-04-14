using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class LeaderboardGtdButtons : MonoBehaviour
{
    [SerializeField] Button _gtdButtonTemplate;

    public UnityEvent<string> OnGtdButtonClicked;

    Button _currentActiveGtdButton;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        _gtdButtonTemplate.gameObject.SetActive(false);
    }

    public void SetGtds(List<string> gtds)
    {
        foreach (var gtd in gtds)
        {
            var button = Instantiate(_gtdButtonTemplate, _gtdButtonTemplate.transform.parent);
            button.gameObject.SetActive(true);
            button.GetComponentInChildren<Text>().text = gtd;
            button.onClick.AddListener(() =>
            {
                OnGtdButtonClicked?.Invoke(gtd);

                // active previous button
                if(_currentActiveGtdButton)
                    _currentActiveGtdButton.interactable = true;

                // disactive current button
                _currentActiveGtdButton = button;
                _currentActiveGtdButton.interactable = false;
            });

            button.interactable = true;
            // click the first button
            if(_currentActiveGtdButton == null)
                button.onClick.Invoke();
        }
    }
}