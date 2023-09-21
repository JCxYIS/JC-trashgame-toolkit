using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class ResourcesUtil
{
    /// <summary>
    /// 從 Resources Instantiate 一個 GameObject
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static GameObject InstantiateFromResources(string key)
    {
        return MonoBehaviour.Instantiate(Resources.Load<GameObject>(key));
    }

    /* -------------------------------------------------------------------------- */

    /// <summary>
    /// same as Resources.Load(key), but if not exist, also attempt to load Resources.Load(key+"(JCTTKDEFAULT)")
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static GameObject LoadFromResourcesJCTTK(string key)
    {
        GameObject go = Resources.Load<GameObject>(key);

        if(go == null)
        {
            go = Resources.Load<GameObject>(key+"(JCTTKDEFAULT)");          
            if(go != null)  
                Debug.Log($"Loaded JCTTK default asset, you can create your custom copy by cloning Resources/{key}(JCTTKDEFAULT) to Resources/{key}");
        }
        return go;
    }

    public static GameObject InstantiateFromResourcesJCTTK(string key)
    {
        GameObject go = LoadFromResourcesJCTTK(key);
        if(go == null)
            throw new System.NotImplementedException($"Neither {key} nor {key}(JCTTKDEFAULT) not exist in Resources!!");
        return MonoBehaviour.Instantiate(go);
    }
}
