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
    private PlayerEmotionControl _emotionControl;
    private FadeSystem fadeSystem;
    private SpriteRenderer _spriteRenderer;

    private PlayerColor _playerColor;

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
           
        if( photonView.isMine)
        {
            cam = GameObject.FindWithTag("MainCamera");
            cam.GetComponent<CamFollow>().target = this.gameObject.transform;

            // PhotonNetwork.player.CustomProperties["Color"] = NetworkManager.Instance.GetPlayerColor();
        }
    }

    private void Start()
    {
        InitializeComponents();
        InitializeWidgets();

        _playerJump.SetPlaySounds(photonView.isMine);
    }

    private void InitializeWidgets()
    {
        if (!photonView.isMine)
        {
            GameObject instance = Instantiate(Resources.Load("Prefabs/Indicator")) as GameObject;

            if (instance != null)
            {
                var indicator = instance.GetComponent<Indicator>();

                indicator.Setup(this.gameObject);
            }
        }
    }


    private void InitializeComponents()
    {
        _playerJump = GetComponent<PlayerJump>();
        _playerPushHand = GetComponent<PlayerPushHand>();   
        _playerMove = GetComponent<PlayerMove>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _emotionControl = GetComponent<PlayerEmotionControl>();

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
        EmotionInput();
    }

    [PunRPC]
    void RPC_DoPush()
    {
        if( _playerPushHand != null)
        {
            _playerPushHand.PushEvent();
        }
    }

    [PunRPC]
    void RPC_Emote(EmotionType emotionType)
    {
        const string emotion_name = "Prefabs/Player/Emotion";
        Vector3 additional_spawn_position = new Vector3(0, 0.35f, 0);

        Vector3 spawn_position = gameObject.transform.position;
        spawn_position += additional_spawn_position;

        var origin_object = Resources.Load(emotion_name) as GameObject;

        var emotion = Instantiate(origin_object, spawn_position, new Quaternion(), gameObject.transform);
        var emotion_viewer = emotion.GetComponent<EmotionViewer>();

        if( emotion_viewer != null)
        {
            emotion_viewer.SetupEmotion(emotionType, 2.0f);
        }
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
            photonView.RPC("RPC_DoPush", PhotonTargets.All);
        }
    }

    private void EmotionInput()
    {
        if( Input.GetKeyDown(KeyCode.T))
        {
            _emotionControl.DoEmote(this);
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
    
    
