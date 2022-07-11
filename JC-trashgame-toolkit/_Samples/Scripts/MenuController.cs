using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] Button _goGameButton;    

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _goGameButton.onClick.AddListener(OnGoGameButtonClicked);
    }

    /* -------------------------------------------------------------------------- */

    void OnGoGameButtonClicked()
    {        
        GameManager.Instance.GoScene("JC_Main");
    }

    
}