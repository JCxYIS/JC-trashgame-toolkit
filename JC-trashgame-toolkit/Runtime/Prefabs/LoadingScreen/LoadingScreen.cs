using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JC.Utility;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

/// <summary>
/// Loading Screen
/// Call SetProgress() to display
/// </summary>
public class LoadingScreen : MonoBehaviour
{
    private static LoadingScreen Instance;
    
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] Slider _progressBar;
    [SerializeField] Text _progressText;
    [SerializeField] Text _contextText;

    private float _currentProgress;
    private float _targetProgress;

    /* -------------------------------------------------------------------------- */


    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        // progress
        _currentProgress = Mathf.Lerp(_currentProgress, _targetProgress, 0.050f);
        _currentProgress = Mathf.MoveTowards(_currentProgress, _targetProgress, 0.005f);
        _progressBar.value = _currentProgress;
        _progressText.text = (100 * _currentProgress).ToString("0.00") + "%";

        // visibility
        if(_currentProgress < 1) // fade in
        {
            _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, 1, 0.15f);
            _canvasGroup.blocksRaycasts = true;
        }
        else // fade out
        {
            _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, 0, 0.25f);
            _canvasGroup.blocksRaycasts = false;
        }
    }

    /* -------------------------------------------------------------------------- */

    
    private void SetProgressSelf(float progress)
    {        
        if(progress == 0)
            _currentProgress = 0;
        _targetProgress = progress;        
    }

    private void SetContextSelf(string context)
    {
        if(!string.IsNullOrEmpty(context))
            _contextText.text = context;
    }


    /* -------------------------------------------------------------------------- */

    static bool loaded = false;
    private static void EnsureInstance()
    {
        // if no instance created, load & create it!
        if(!loaded)
        {
            var g = Addressables.InstantiateAsync("Loading UI").WaitForCompletion();
            Instance = g.GetComponent<LoadingScreen>();
            DontDestroyOnLoad(Instance.gameObject);
            loaded = true;
        }
    }


    /// <summary>
    /// Show a Loading screen
    /// </summary>
    /// <param name="progress">If progress >= 1, hide</param>
    /// <param name="context">text to indicate what are you doing</param>
    public static void SetProgress(float progress, string context)
    {           
        // value check
        if(progress <= 0) 
            progress = 0;
        else if(progress > 1) 
            progress = 1;

        // haha lets go
        EnsureInstance();
        Instance.SetProgressSelf(progress);
        Instance.SetContextSelf(context);
    }

    /// <summary>
    /// Show a Loading screen
    /// </summary>
    /// <param name="progress">If progress >= 1, hide</param>
    public static void SetProgress(float progress)
    {
        SetProgress(progress, null);
    }

    /// <summary>
    /// Show a Loading screen
    /// </summary>
    /// <param name="context">text to indicate what are you doing</param>
    /// <param name="progressList">A list of progress of many tasks. We will sum up and calc average for the real progress</param>
    public static void SetProgress(params float[] progressList)
    {
        float progress = 0;
        foreach(var p in progressList)
        {
            if(p > 1)
                progress += 1;
            else if(p < 0)
                progress += 0;
            else
                progress += p;
        }
        progress /= progressList.Length;
        SetProgress(progress);
    }


    /// <summary>
    /// Set Loading context string
    /// </summary>
    /// <param name="context"></param>
    public static void SetContext(string context)
    {
        EnsureInstance();
        Instance.SetContextSelf(context);
    }


    /// <summary>
    /// return if the loading screen is showing 
    /// /// </summary>
    /// <value></value>
    public static bool IsFullyShown
    {
        get
        {
            EnsureInstance();
            return Instance._canvasGroup.alpha == 1;
        }
    }

}