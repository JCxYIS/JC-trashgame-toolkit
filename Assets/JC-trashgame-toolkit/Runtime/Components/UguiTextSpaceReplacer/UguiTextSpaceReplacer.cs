using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UguiTextSpaceReplacer : MonoBehaviour
{
    Text text;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        text = GetComponent<Text>();               
    }

    void LateUpdate()
    {
        text.text = text.text.Replace(' ', '\u00A0');
    }
}
