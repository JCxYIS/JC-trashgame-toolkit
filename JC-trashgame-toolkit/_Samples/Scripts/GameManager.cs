using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{


    /* -------------------------------------------------------------------------- */
    Coroutine _goSceneCoroutine;

    public void GoScene(string sceneName)
    {
        if(_goSceneCoroutine == null)
        {
            StartCoroutine(GoSceneCoroutine(sceneName));
        }
        else
        {
            Debug.LogWarning("[GameManager] GoScene: Coroutine is already running.");
        }
    }

    IEnumerator GoSceneCoroutine(string sceneName)
    {
        var loadGame = SceneManager.LoadSceneAsync(sceneName);

        //
        while(!loadGame.isDone) 
        {
            LoadingScreen.SetContext("Loading...");
            LoadingScreen.SetProgress(loadGame.progress * 0.9f);  // capped to 0.9
            yield return null;
        }


        // For demo purpose, let's wait 1s here
        // Notice: the main scene is started at this time
        float demoWait = 1f;
        for(float t = 0; t < demoWait; t += Time.deltaTime)
        {
            LoadingScreen.SetProgress(0.9f + t/demoWait*0.1f, $"Let's wait 3 seconds: {t.ToString("0.0")}/{demoWait}");
            yield return null;
        }

        LoadingScreen.SetProgress(1); // load complete! let's hide the loading screen
        _goSceneCoroutine = null;
    }

    /* -------------------------------------------------------------------------- */
}