using UnityEngine;
using System.Collections;
using System.Threading;


/// <summary>
/// 單實例模式
/// ref: https://www.cnblogs.com/fastcam/p/5924036.html
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class MonoSingleton<T> : MonoBehaviour
where T : MonoSingleton<T>
{

    private static T m_Instance = null;
    private static string name;
    private static Mutex mutex;
    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                if (IsSingle())
                {
                    m_Instance = new GameObject(name, typeof(T)).GetComponent<T>();
                    m_Instance.SingletonInit();
                }
            }
            return m_Instance;
        }
    }

    private static bool IsSingle()
    {
        bool createdNew;
        name = "Singleton of " + typeof(T).ToString();
        mutex = new Mutex(false, name, out createdNew);
        return createdNew;
    }

    private void Awake()
    {
        if (m_Instance == null)
        {
            if (IsSingle())
            {
                m_Instance = this as T;
                m_Instance.SingletonInit();
            }
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnDestory()
    {
        if (m_Instance != null)
        {
            mutex.ReleaseMutex();
            SingletonDestroy();
            m_Instance = null;
        }
    }
    private void OnApplicationQuit()
    {
        mutex.ReleaseMutex();
    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void SingletonInit()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void SingletonDestroy()
    {

    }
}