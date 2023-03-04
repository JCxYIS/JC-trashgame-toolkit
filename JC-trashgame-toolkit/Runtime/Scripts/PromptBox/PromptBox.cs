using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
// using UnityEngine.AddressableAssets;

/// <summary>
/// Prompt box or Message box.
/// Call CreateMessageBox() to display a simple message box,
/// or Create() for manually setup.
/// /// </summary>
public class PromptBox : PopUI
{
    [SerializeField] Text _titleText; 
    [SerializeField] Text _contentText; 
    [SerializeField] Button _confirmButton;
    [SerializeField] Button _cancelButton;

    /// <summary>
    /// The settings of this Prompt Box 
    /// </summary>
    private PromptBoxSettings _settings;
    


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        // gameObject.SetActive(false);        
        base.Awake();
    }

    /* -------------------------------------------------------------------------- */


    /// <summary>
    /// Display the prompt box
    /// </summary>
    /// <param name="settings"></param>
    public void Show(PromptBoxSettings settings)
    {
        RefreshSettings(settings);
        
        // on
        // gameObject.SetActive(true);
        base.Show();
    }

    public override void Show()
    {
        throw new System.NotSupportedException("Use Show(PromptBoxSettings) instead");
    }

    public override void Hide()
    {
        // gameObject.SetActive(false);
        // Destroy(gameObject, 1);
        // Addressables.Release(gameObject);
        base.Hide();
    }

    /// <summary>
    /// Display the prompt box
    /// </summary>
    /// <param name="settings"></param>
    public void RefreshSettings(PromptBoxSettings settings)
    {
        _settings = settings;

        // Texts
        _titleText.text = settings.Title;
        _contentText.text = settings.Content;

        // Callbacks
        _confirmButton.onClick.RemoveAllListeners();
        _confirmButton.onClick.AddListener(OnConfirmButtonClick);
        _cancelButton.onClick.RemoveAllListeners();
        _cancelButton.onClick.AddListener(OnCancelButtonClick);

        // Button Texts
        if(_settings.ConfirmButtonText == "")
        {
            _confirmButton.gameObject.SetActive(false);
        }
        else
        {
            Text confirmButtText = _confirmButton.transform.GetChild(0).GetComponent<Text>();
            confirmButtText.text = _settings.ConfirmButtonText;
        }
        if(_settings.CancelButtonText == "")
        {
            _cancelButton.gameObject.SetActive(false);
        }
        else
        {
            Text cancelButtText = _cancelButton.transform.GetChild(0).GetComponent<Text>();
            cancelButtText.text = _settings.CancelButtonText;
        }
        
        // sfx
        //  = _settings.ShowSfx;        
        //  = _settings.ButtonSfx;    

        // 
        _hideOnEscClicked = _settings.CanUseEscToCancel;    
    }


    /* -------------------------------------------------------------------------- */

    public void OnConfirmButtonClick()
    {
        _settings.ConfirmCallback?.Invoke();
        Hide();
    }

    public void OnCancelButtonClick()
    {
        _settings.CancelCallback?.Invoke();
        Hide();
    }

    /* -------------------------------------------------------------------------- */

    /// <summary>
    /// Create a prompt box 
    /// </summary>
    /// <param name="settings">PromptBoxSettings</param>
    public static void Create(PromptBoxSettings settings)
    {
        var g = ResourcesUtil.InstantiateFromResources("Prefabs/PromptBox");
        PromptBox promptBox = g.GetComponent<PromptBox>();
        promptBox.Show(settings);
    }

    /// <summary>
    /// Create a prompt box (message + Confirm and Cancel btn)
    /// </summary>
    /// <param name="settings">PromptBoxSettings</param>
    public static void CreatePrompt(string message, UnityAction confirmCallback, PromptBoxSettings additionalSettings = null)
    {
        if(additionalSettings == null)
            additionalSettings = new PromptBoxSettings();
        additionalSettings.Content = message;
        additionalSettings.ConfirmCallback = confirmCallback;
        Create(additionalSettings);
    }

    /// <summary>
    /// Create a message box (message + confirm btn)
    /// </summary>
    /// <param name="message"></param>
    /// <param name="callback"></param>
    public static void CreateMessageBox(string message, UnityAction callback = null)
    {
        print($"<color=green>[MSG BOX]" + (callback==null?"":"(with callback)") + $"</color> {message}");
        Create(new PromptBoxSettings{
            Content = message,
            ConfirmCallback = callback,
            CancelButtonText = "",
            CanUseEscToCancel = callback == null,
        });
    }
}
