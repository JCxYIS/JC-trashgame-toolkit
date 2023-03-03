using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Mono singleton Class. Extend this class to make singleton component.
/// Notice that this class is not thread safe, and you may want to add "DontDestroyOnload" to the singleton object.
/// </summary>
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T m_Instance = null;
    public static T Instance
    {
        get
        {
            // Instance required for the first time, we look for it
            if(!m_Instance) 
            {
                m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;

                // Object not found, we create a temporary one
                if( m_Instance == null )
                {
                    string tString = typeof(T).ToString();
  				    // Debug.Log("No instance of " + tString + ", one is created.");

                    // Look for template in Resources folder
                    GameObject template = Resources.Load<GameObject>("Prefabs/"+tString);
                    if(template == null)
                    {
                        m_Instance = new GameObject("[Instance] " + tString, typeof(T)).GetComponent<T>();
                    }
                    else
                    {
                        m_Instance = Instantiate(template).GetComponent<T>();
                        m_Instance.name = "[Instance] " + tString + "(Instantiated from Resources/Prefabs)";
                    }

                    // Problem during the creation, this should not happened
                    if( m_Instance == null )
                    {
                        throw new System.Exception("Problem during the creation of " + tString);
                    }                    
                }                
                // Object not initaited, we init it
				if (!_isInitialized)
                {
					_isInitialized = true;
					m_Instance.Init();
				}
            }
            return m_Instance;
        }
    }

	private static bool _isInitialized;

    // If no other monobehaviour request the instance in an awake function
    // executing before this one, no need to search the object.
    protected virtual void Awake()
    {
        if (m_Instance == null) {
			m_Instance = this as T;
		} else if (m_Instance != this) {
			Debug.LogError ("Another instance of " + GetType () + " is already exist! Destroying self...");
			DestroyImmediate (this);
			return;
		}
		if (!_isInitialized) {
			DontDestroyOnLoad(gameObject);
			_isInitialized = true;
			m_Instance.Init ();
		}
    }
	
	
    /// <summary>
    /// This function is called when the instance is used the first time
    /// Put all the initializations you need here, as you would do in Awake
    /// </summary>
	protected virtual void Init(){}
 
    /// Make sure the instance isn't referenced anymore when the user quit, just in case.
    private void OnDestroy()
    {
        m_Instance = null;
        _isInitialized = false;
    }
}