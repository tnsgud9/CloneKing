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
        private FadeSystem fadeSystem;
        
        public GameObject cam;

        // MoveInput Variables
        private float horizontal = 0f;
        
        
        void Awake()
        {
            GameManager.Instance.AddPlayer(this.gameObject);

            if (fadeSystem == null)
            {
                fadeSystem = gameObject.AddComponent<FadeSystem>();
            }
           
            cam = GameObject.FindWithTag("MainCamera");
            cam.GetComponent<CamFollow>().target = this.gameObject.transform;
        }

        private void Start()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            _playerJump = GetComponent<PlayerJump>();
            _playerPushHand = GetComponent<PlayerPushHand>();
            _playerMove = GetComponent<PlayerMove>();
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

        
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Trigger Enter");
            if(other.CompareTag("Goal"))
                GameManager.Instance.ReachGoalEvent(this.gameObject);
        }


        private IEnumerator waitThenCallback(float time, Action callback)
        {
            yield return new WaitForSeconds(time);
            callback();
        }
    }
    
    
}
