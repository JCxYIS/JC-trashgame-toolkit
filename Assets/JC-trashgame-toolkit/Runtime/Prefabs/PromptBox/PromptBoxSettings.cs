using UnityEngine.Events;

/// <summary>
/// Hold prompt box creation settings
/// Avaliable settings:
/// Title: string
/// Content: string
/// ConfirmButtonText: string
/// CancelButtonText: string
/// ConfirmButtonCallback: UnityEvent
/// CancelButtonCallback: UnityEvent
/// </summary>
public class PromptBoxSettings
{
    public string Title = "Confirmation";

    public string Content = "Are you sure?";

    public UnityAction ConfirmCallback = null;

    public UnityAction CancelCallback = null;    

    public string ConfirmButtonText = "確認";
    
    /// <summary>
    /// Set to "" (empty string) will hide the cancel button
    /// </summary>
    public string CancelButtonText = "取消";

    public bool CanUseEscToCancel = true;
}