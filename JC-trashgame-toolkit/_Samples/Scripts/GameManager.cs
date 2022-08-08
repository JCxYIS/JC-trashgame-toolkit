using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace JC.TrashGameToolkit.Sample
{
    public class GameManager : MonoSingleton<GameManager>
    {
        /// <summary>
        /// Call when the scene is loaded
        /// </summary>
        public UnityAction OnSceneLoaded;

        Coroutine _goSceneCoroutine;

        /* -------------------------------------------------------------------------- */

        /// <summary>
        /// Start Scene Transition
        /// </summary>
        /// <param name="sceneName"></param>
        public void GoScene(string sceneName, bool useLoadingScreen = false)
        {
            if (_goSceneCoroutine != null)
            {
                Debug.LogWarning("[GameManager] GoScene: Coroutine is already running.");
                return;
            }

            StartCoroutine(GoSceneCoroutine(sceneName, useLoadingScreen));
                        
        }

        IEnumerator GoSceneCoroutine(string sceneName, bool useLoadingScreen)
        {
            // do load
            var loadGame = SceneManager.LoadSceneAsync(sceneName);
            loadGame.allowSceneActivation = false;
            while (!loadGame.isDone)
            {
                if(useLoadingScreen)
                {
                    LoadingScreen.SetContext("Loading...");
                    LoadingScreen.SetProgress(loadGame.progress);  // this is capped to 0.9                    
                }                

                if (!useLoadingScreen || LoadingScreen.IsFullyShown) // prevent scene is loaded faster than load screen faded in
                {
                    loadGame.allowSceneActivation = true;
                }
                yield return null;
            }

            // For demo purpose, we can wait 1s here
            // Notice: the main scene is started at this time
            // float demoWait = 1f;
            // for (float t = 0; t < demoWait; t += Time.deltaTime)
            // {
            //     LoadingScreen.SetProgress(0.9f + t / demoWait * 0.1f, $"Let's wait {demoWait} seconds: {t.ToString("0.0")}/{demoWait}");
            //     yield return null;
            // }

            LoadingScreen.SetProgress(1); // load complete! let's hide the loading screen
            yield return new WaitForSeconds(.5f);

            _goSceneCoroutine = null;
            OnSceneLoaded?.Invoke();
        }

        /* -------------------------------------------------------------------------- */

        public void GameOver(GameScore score)
        {
            // Score = score;
            // GoScene("JC_Result");
            var g = Addressables.InstantiateAsync("Sample_ResultPanel UI").WaitForCompletion();
            g.GetComponent<ResultUI>().Show(score);
        }
    }
}