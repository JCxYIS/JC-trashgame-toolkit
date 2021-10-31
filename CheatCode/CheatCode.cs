using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// Inherit this class, and write down the 
/// </summary>
public class CheatCode : MonoBehaviour
{
    [Header("起動密技的按鍵 set")]
    public List<KeyCode> ActivationCodes;

    /// <summary>
    /// When the activate code has input
    /// </summary>
    public UnityEvent OnActivate;

    private int currentActivation = 0;



    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(Input.GetKeyDown(ActivationCodes[currentActivation]))
        {
            currentActivation++;

            if(currentActivation >= ActivationCodes.Count)
            {
                print("<color=cyan>CHEAT CODE ACTIVATED!</color> "+name);
                OnActivate?.Invoke();
                currentActivation = 0;
            }
        }
        else if(Input.anyKeyDown)
        {
            currentActivation = 0;
        }
    }
}