using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
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
        
        
        #region Timer Variables
        private IEnumerator Timer;
        private int timeCount = 0;
        private int hour=0, minute=0, second=0;
        #endregion
        
        // Comment : 타이머의 관련된 변수는 추후에 networkManager에서 관리가 필요합니다.
        //           접속시점이 기준이 아님.


        private void Start()
        {
            Timer = TimeCoroutine();
            StartCoroutine(Timer);

        }
        
        

        // PlayerController.cs의 void start()에서 추가 됩니다.
        public void AddPlayer(GameObject player)
        {
            players.Add(player);
        }
        
        
        
        public void ReachGoalEvent(GameObject player)
        {
            foreach (GameObject obj in players)
            {
                obj.GetComponent<PlayerController>().enabled = false;
            }
        }

        
        private IEnumerator TimeCoroutine()
        {
            while (true)
            {
                timeCount++;
                hour = (timeCount%(60*60*24))/(60*60); 
                minute = (timeCount%(60*60))/(60);
                second = timeCount%(60);
                //timerText.text = hour + ":" + minute + ":" + second;
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
