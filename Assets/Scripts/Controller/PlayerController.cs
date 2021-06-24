using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class PlayerController : MonoBehaviour
    {
        
        //Todo : 불필요한 변수들 제거 필요.
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
        
        //Todo: 타이머 기능 GameManager.cs로 이동 필요.
        public int timer = 0;
        private int hour=0, minute=0, second=0;
        private IEnumerator coroutine;
        private static readonly int Goal = Animator.StringToHash("Goal");

        public AudioClip goalSound;

        // MoveInput Variables
        private float horizontal = 0f;
        
        
        void Awake()
        {
            GameManager.Instance.AddPlayer(this.gameObject);

            if (fadeSystem == null)
            {
                fadeSystem = gameObject.AddComponent<FadeSystem>();
            }
            
            // Todo: GameObject.Find 함수 변경 필요 ( 느리다 )
            carmera = GameObject.Find("Main Camera");
            carmera.GetComponent<CamFollow>().target = this.gameObject.transform;
            completeTexts = carmera.transform.GetChild(0).transform.Find("Complete Objects").gameObject;
            timerText = carmera.transform.GetChild(0).transform.Find("timer text").gameObject.GetComponent<Text>();
        }

        private void Start()
        {
            InitializeComponents();

            coroutine = GameTimer();
            
            completeTexts.SetActive(false);
            StartCoroutine(coroutine);
        }

        private void InitializeComponents()
        {
            _playerJump = GetComponent<PlayerJump>();
            _playerPushHand = GetComponent<PlayerPushHand>();
            _playerMove = GetComponent<PlayerMove>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            JumpInput();
            MoveInput();
            PushInput();
        }

        private void JumpInput()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                _playerJump.JumpEvent(JumpState.Ready);
            }

            if(Input.GetKeyUp(KeyCode.Space))
            {
                _playerJump.JumpEvent(JumpState.Jump);
            }

        }

        private void MoveInput()
        {
            if (!_playerJump.isJumped())
            {
                horizontal = Input.GetAxisRaw("Horizontal");
                _playerMove.MoveEvent(horizontal);
            }
        }
        
        private void PushInput()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                _playerPushHand.PushEvent();
            }
        }
        
        
        //Todo: 타이머 GameManager로 이전
        
        private void FinishGame()
        {
            _audioSource.clip = goalSound;
            _audioSource.Play();

           // _playerJump.state = JumpState.Goal;
            _playerJump.enabled = false;
            _playerPushHand.enabled = false;
            _rigidbody2D.gravityScale = 0;
            _boxCollider2D.enabled = false;
            StartCoroutine(waitThenCallback(0.1f, () =>
            {
                _playerMove.enabled = false;
                _animator.SetBool("isMove", false);
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
                fadeSystem.textFadeOutRetro(e, 0.1f, 0.25f);
            }
            StopCoroutine(coroutine);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag.Equals("Goal"))
            {
                GameManager.Instance.ReachGoalEvent(this.gameObject);
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
