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
    private BaseSkill _playerSkill;
    private PlayerJump _playerJump;
    private PlayerEmotionControl _emotionControl;
    private FadeSystem fadeSystem;
    private SpriteRenderer _spriteRenderer;

    //Network
    private PhotonTransformView _photonTransformView;

    private PlayerColor _playerColor;

    public GameObject cam;

    // MoveInput Variables
    private float horizontal = 0f;

    public Vector2 GetForwardVector2D()
    {
        if( _spriteRenderer != null && _spriteRenderer.flipX)
        {
            return new Vector2(-1.0f, 0.0f);
        }

        return new Vector2(1.0f, 0.0f);
    }

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

                bool syncModeEnable = false;
                bool directlySync = false;

                switch ( _playerJump.GetJumpState())
                {
                    case JumpState.Ready:
                        syncModeEnable = true;
                        break;

                    case JumpState.Ground:
                        directlySync = true;
                        break;
                }

                _photonTransformView.SetForceSyncMode(syncModeEnable, directlySync);
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
            BindMainCameraTarget();

            // PhotonNetwork.player.CustomProperties["Color"] = NetworkManager.Instance.GetPlayerColor();
        }
    }
    public void Initialize()
    {
        InitializeComponents();
        InitializeSkills();
        InitializeWidgets();

        _playerJump.SetPlaySounds(photonView.isMine);
    }

    public void BindMainCameraTarget()
    {
        cam = GameObject.FindWithTag("MainCamera");
        cam.GetComponent<CamFollow>().target = this.gameObject.transform;
    }

    private void Start()
    {
        Initialize();
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
        _playerSkill = GetComponent<BaseSkill>();
        _playerMove = GetComponent<PlayerMove>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _emotionControl = GetComponent<PlayerEmotionControl>();
        _photonTransformView = GetComponent<PhotonTransformView>();

        if ( !photonView.isMine)
        {
            _playerJump.enabled = false;
            _playerMove.enabled = false;

            _spriteRenderer.sortingOrder = NetworkManager.Instance.AssignNetworkIndex();
        }
        else
        {
            const int sortingMax = 10000;

            _spriteRenderer.sortingOrder = sortingMax;
        }
    }

    private void Update()
    {
        if (!photonView.isMine)
            return;

        PopupInput();
        JumpInput();
        MoveInput();
        SkillInput();
        EmotionInput();
    }

    [PunRPC]
    void RPC_FinishGame( bool _is_victory )
    {
        Manager.GameManager.Instance.ReachGoalEvent(this.gameObject, !_is_victory);

        if( _is_victory)
        {
            BindMainCameraTarget();
        }
    }

    [PunRPC]
    void RPC_SpawnObject( string prefabName, double spawnTime, Vector3 position)
    {
        GameObject go = Resources.Load(prefabName) as GameObject;

        GameObject SpawnObject = Instantiate(go, position, new Quaternion());

        if (SpawnObject != null)
        {
            SpawnObject.GetComponent<SynchronizedObject>().SetupObject(this, spawnTime);
            SpawnObject.transform.position = position;
        }
    }

    [PunRPC]
    void RPC_DoSkill()
    {
        if(_playerSkill != null)
        {
            _playerSkill.DoSkill();
        }
    }

    [PunRPC]
    public void RPC_Emote(EmotionType emotionType)
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
            emotion_viewer.SetupEmotion(emotionType, 2.5f);
        }
    }

    private void InitializeSkills()
    {
        if(PhotonNetwork.offlineMode)
        {
            return;
        }

        var prevSkill = GetComponent<BaseSkill>();
        if (prevSkill != null )
        {
            Destroy(prevSkill);
        }

        int outParam = 0;
        photonView.TryGetValueToInt("SkillType", out outParam);

        if(outParam < 0)
        {
            outParam = 0;
        }
        SkillType equipSkill = (SkillType)outParam;

        Type skillType = typeof(PlayerPushHand);
        switch (equipSkill)
        {
            case SkillType.PushHand:
                skillType = typeof(PlayerPushHand);
                break;

            case SkillType.SelfExplosion:
                skillType = typeof(SelfExplosionSkill);
                break;

            case SkillType.SpawnVine:
                skillType = typeof(SpawnSkill);
                break;

            case SkillType.DoubleJump:
                skillType = typeof(DoubleJumpSkill);
                break;

            case SkillType.Teleport:
                skillType = typeof(TeleportSkill);
                break;

        }

        var baseSkill = gameObject.AddComponent(skillType) as BaseSkill;

        _playerSkill = baseSkill;
        _playerSkill.BindPlayerController(this);
    }

    private void PopupInput()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Manager.GameManager.Instance.CreateExitPopup();
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
        
    private void SkillInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (_playerSkill.IsExpiredCooltime())
            {
                _playerSkill.DoSkill();
                photonView.RPC("RPC_DoSkill", PhotonTargets.All);
            }
        }
    }

    private void EmotionInput()
    {
        if( Input.GetKeyDown(KeyCode.T))
        {
            _emotionControl.DoEmote(this, EmotionType.ThumbsUp);
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Goal"))
        {
            if( PhotonNetwork.offlineMode)
            {
                Manager.GameManager.Instance.ReachGoalEvent(this.gameObject,false);
            }
            else
            {
                photonView.RPC("RPC_FinishGame", PhotonTargets.All, true);
            }
        }
        
    }

    private IEnumerator waitThenCallback(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }
}
    
    
