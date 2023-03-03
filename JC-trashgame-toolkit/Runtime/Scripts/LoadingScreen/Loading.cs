using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Loading : MonoSingleton<Loading>
{
    /// <summary>
    /// Current Progress
    /// </summary>
    protected float _targetProgress = 0;

    /// <summary>
    /// Loading Context (Reason for loading)
    /// </summary>
    protected string _contextText = "Loading...";

    [SerializeField] CanvasGroup _canvasGroup;

    /* -------------------------------------------------------------------------- */
    protected override void Init()
    {
        _canvasGroup.alpha = 0;
    }

    protected void FixedUpdate()
    {
        _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, _targetProgress, 0.25f);
    }

    /* -------------------------------------------------------------------------- */
    
    public void SetLoading(bool isLoading)
    {
        _targetProgress = isLoading ? 1 : 0;
    }
}