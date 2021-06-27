using System;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class SingletonPhoton<T> : Photon.PunBehaviour where T : Photon.PunBehaviour
    {
        private static T _instance = null;

        protected void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        var go = new GameObject();
                        var component = go.AddComponent<T>();

                        _instance = component;
                    }

                    _instance.gameObject.name = typeof(T).ToString();

                    DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }
        }
    }

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
                        var go = new GameObject();
                        var component = go.AddComponent<T>();

                        go.name = typeof(T).ToString();

                        DontDestroyOnLoad(go);

                        _instance = component;
                    }
                }

                return _instance;
            }
        }
    }
    
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private List<GameObject> players;

        public void AddPlayer(GameObject player)
        {
          //  players.Add(player);

        }

        public void CreateNewPlayer()
        {
            const string player_prefab_name = "Prefabs/PlayerChara";
            Vector3 start_location = new Vector3(0, 0, 0);

            PhotonNetwork.Instantiate(player_prefab_name, start_location, Quaternion.identity, 0);
        }

    }
}
