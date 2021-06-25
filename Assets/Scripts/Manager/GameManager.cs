using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private Text timeText;

        public GameObject player;
        
        
        #region Timer Variables
        private IEnumerator _gameTimer;
        public int timeCount = 0;
        private int _hour=0, _minute=0, _second=0;
        #endregion
        
        // Comment : 타이머의 관련된 변수는 추후에 networkManager에서 관리가 필요합니다.
        //           접속시점이 기준이 아님.
        private void Start()
        {
            InitializeComponents();
            _gameTimer = TimeCoroutine();
            StartCoroutine(_gameTimer);
            SpawnPlayer(); // Todo: 차후 네트워크 기능 추가 이후에는 변경해주세요.
        }

        private void InitializeComponents()
        {
        }


        // PlayerController.cs의 void start()에서 추가 됩니다.
        public void AddPlayer(GameObject player)
        {
            players.Add(player);
        }
        
        public void ReachGoalEvent(GameObject player)
        {
            StopCoroutine(_gameTimer);
            foreach (GameObject obj in players)
            {
                obj.GetComponent<PlayerController>().enabled = false;
            }
        }
        
        //  Todo: 네트워크 접속이 될때 플레이어 생성 함수를 호출하면 될거 같습니다.
        public void SpawnPlayer()
        {
            StartCoroutine(waitThenCallback(1f,() =>
            {
                Instantiate(player);
            }));
        }

        private IEnumerator TimeCoroutine()
        {
            while (true)
            {
                timeCount++;
                _hour = (timeCount%(60*60*24))/(60*60); 
                _minute = (timeCount%(60*60))/(60);
                _second = timeCount%(60);
                timeText.text = _hour + ":" + _minute + ":" + _second;
                yield return new WaitForSeconds(1f);
                
            }
        }
        //====
        private IEnumerator waitThenCallback(float time, Action callback)
        {
            yield return new WaitForSeconds(time);
            callback();
        }
    }
    
    
}
