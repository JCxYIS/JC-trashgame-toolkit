using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoSceneFunctionContainer : MonoBehaviour
{
    public void GoScene(string sceneName)
    {
        GameManager.Instance.GoScene(sceneName);
    }
}