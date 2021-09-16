using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
        
    public static T Instance
    {
        get
        {
            if( _instance == null)
            {
                _instance = (T)FindObjectOfType<T>();

                if( _instance == null)
                {
                    var obj = new GameObject();
                    var component = obj.AddComponent<T>();

                    obj.name = typeof(T).ToString();

                    DontDestroyOnLoad(obj);

                    _instance = component;
                }
            }

            return _instance;
        }
    }
}
