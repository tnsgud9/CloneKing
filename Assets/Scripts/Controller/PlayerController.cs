using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Realtime;

public class PlayerController : Photon.PunBehaviour, IPunObservable
{
    //Todo : 불필요한 변수들 제거 필요.
    private PlayerMove _playerMove;
    private PlayerPushHand _playerPushHand;
    private PlayerJump _playerJump;
    private FadeSystem fadeSystem;
    private SpriteRenderer _spriteRenderer;
        
     public GameObject cam;

    // MoveInput Variables
    private float horizontal = 0f;

    public override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        base.OnPhotonInstantiate(info);

        info.sender.TagObject = this.gameObject;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if( stream.isWriting)
        {
            stream.SendNext(_spriteRenderer.flipX);
            stream.SendNext(_playerJump.GetJumpState());
            stream.SendNext(PhotonNetwork.playerName);
        }
        else
        {
            _spriteRenderer.flipX = (bool)stream.ReceiveNext();

            var jump_state = (JumpState)stream.ReceiveNext();

            if( jump_state != _playerJump.GetJumpState())
            {
                _playerJump.ChangeJumpState(jump_state);
            }
		}
	}

	void Awake()
	{
		 Manager.GameManager.Instance.AddPlayer(this.gameObject);
		
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
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if ( !photonView.isMine)
        {
            _playerJump.enabled = false;
            _playerMove.enabled = false;
        }
    }

    private void Update()
    {
        if (!photonView.isMine)
            return;


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
                Manager.GameManager.Instance.ReachGoalEvent(this.gameObject);
        }

    private IEnumerator waitThenCallback(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }
}
    
    
