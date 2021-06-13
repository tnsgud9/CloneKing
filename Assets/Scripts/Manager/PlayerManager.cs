using System;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class PlayerManager : MonoBehaviour
    {
        private PlayerMove _playerMove;
        private PlayerPushHand _playerPushHand;
        private PlayerJump _playerJump;
        private Rigidbody2D _rigidbody2D;
        private BoxCollider2D _boxCollider2D;
        private FadeSystem fadeSystem;
        private Animator _animator;
        private AudioSource _audioSource;
        
        
        public GameObject carmera;
        public GameObject completeTexts;
        public Text timerText;
        public int timer = 0;
        private int hour=0, minute=0, second=0;
        private IEnumerator coroutine;
        private static readonly int Goal = Animator.StringToHash("Goal");

        public AudioClip goalSound;
        void Awake()
        {
            GameManager.instance.AddPlayer(this.gameObject);
            if (!fadeSystem) fadeSystem = this.gameObject.AddComponent<FadeSystem>();

            carmera = GameObject.Find("Main Camera");
            carmera.GetComponent<CamFollow>().target = this.gameObject.transform;
            completeTexts = carmera.transform.GetChild(0).transform.Find("Complete Objects").gameObject;
            timerText = carmera.transform.GetChild(0).transform.Find("timer text").gameObject.GetComponent<Text>();


        }

        private void Start()
        {
            coroutine = GameTimer();
            _playerJump = GetComponent<PlayerJump>();
            _playerPushHand = GetComponent<PlayerPushHand>();
            _playerMove = GetComponent<PlayerMove>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            
            Debug.Log("asdasdasdas");
            completeTexts.SetActive(false);
            StartCoroutine(coroutine);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Goal")
            {
                Debug.Log("Reach to Goal");

                _audioSource.clip = goalSound;
                _audioSource.Play();
                
                _playerJump.state = PlayerJump.jumpState.Goal;
                _playerJump.enabled = false;
                _playerPushHand.enabled = false;
                _rigidbody2D.gravityScale = 0;
                _boxCollider2D.enabled = false;
                StartCoroutine(waitThenCallback(0.1f, () =>
                {
                    _playerMove.enabled = false;
                    _animator.SetBool("isMove",false);
                    _animator.SetTrigger(Goal);
                }));
                StopCoroutine(GameTimer());

                completeTexts.transform.GetChild(1).GetComponent<Text>().text =
                    "Clear Time : " + hour + ":" + minute + ":" + second;
                completeTexts.SetActive(true);
                
                Text[] texts = completeTexts.transform.GetComponentsInChildren<Text>();
                foreach (Text e in texts)
                {
                    Debug.Log(e);
                    fadeSystem.textFadeOutRetro(e,0.1f,0.25f);
                }
                StopCoroutine(coroutine);

            }
        }

        private IEnumerator GameTimer()
        {
            while (true)
            {
                timer++;
                hour = (timer%(60*60*24))/(60*60); 
                minute = (timer%(60*60))/(60);
                second = timer%(60);
                timerText.text = hour + ":" + minute + ":" + second;
                yield return new WaitForSeconds(1f);
            }
        }
        
        private IEnumerator waitThenCallback(float time, Action callback)
        {
            yield return new WaitForSeconds(time);
            callback();
        }
    }
    
    
}
