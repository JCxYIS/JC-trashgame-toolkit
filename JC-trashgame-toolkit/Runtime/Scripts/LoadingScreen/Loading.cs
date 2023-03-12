using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Loading : MonoSingleton<Loading>
{
    /// <summary>
    /// Current Progress
    /// </summary>
    protected float _targetProgress = 0;

    /// <summary>
    /// Loading Context (Reason for loading)
    /// </summary>
    protected string _contextText = "";

    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] Text _text;

    /* -------------------------------------------------------------------------- */
    protected override void Init()
    {
        _canvasGroup.alpha = 0;
    }

    protected void FixedUpdate()
    {
        _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, _targetProgress, 0.25f);
        _canvasGroup.blocksRaycasts = _canvasGroup.alpha != 0;
        _text.text = _contextText;
    }

    /* -------------------------------------------------------------------------- */
    
    public void SetLoading(bool isLoading, string context = "Loading...")
    {
        _targetProgress = isLoading ? 1 : 0;
        _contextText = context;
    }
}