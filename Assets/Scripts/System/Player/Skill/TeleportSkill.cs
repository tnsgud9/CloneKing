using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportSkill : BaseSkill
{
    private PlayerJump _playerJump = null;
    private PlayerMove _playerMove = null;
    private Rigidbody2D _rigidbody2D = null;
    private BoxCollider2D _boxCollider2D = null;
    private PhotonTransformView _transformView = null;

    public void Start()
    {
        delayTime = 5.0f;
        coolTime = 5.0f;
    }

    public override void BindPlayerController(PlayerController playerController)
    {
        base.BindPlayerController(playerController);

        _playerMove     = playerController.gameObject.GetComponent<PlayerMove>();
        _playerJump     = playerController.gameObject.GetComponent<PlayerJump>();
        _boxCollider2D  = playerController.gameObject.GetComponent<BoxCollider2D>();
        _rigidbody2D    = playerController.gameObject.GetComponent<Rigidbody2D>();
        _transformView  = playerController.gameObject.GetComponent<PhotonTransformView>();
    }

    public void ReleaseHolding()
    {
        _playerJump.enabled = true;
        _playerMove.enabled = true;
        _boxCollider2D.enabled = true;
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        _rigidbody2D.simulated = true;
        _transformView.enabled = true;
    }

    protected override void OnFinishDelayAction()
    {
        base.OnFinishDelayAction();
    }

    protected override void OnActivation()
    {
        base.OnActivation();
    }

    protected override bool OnStartAction()
    {
        base.OnStartAction();

        if (_playerJump.GetJumpState() == JumpState.Ground)
        {
            _playerJump.enabled = false;
            _playerMove.enabled = false;
            _boxCollider2D.enabled = false;
            _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            _rigidbody2D.simulated = false;
            _transformView.enabled = false;

            SpawnTeleportObject();

            return true;
        }

        return false;
    }

    protected void SpawnTeleportObject()
    {
        const string teleportObjectPath = "Prefabs/Object/TeleportCloud";

        if (_playerController != null)
        {
            Vector3 spawnPosition = transform.position;

            spawnPosition += _playerController.GetForwardVector2D().ConvertToVector3D() * 10.0f;
            spawnPosition.y += -5.0f;

            if (_playerController.photonView.isMine)
            {
                GameObject go = PhotonNetwork.Instantiate(teleportObjectPath, spawnPosition, Quaternion.identity, 0);

                if (go != null)
                {
                    go.GetComponent<SynchronizedObject>().SetupObject(_playerController, PhotonNetwork.time);
                }
            }
        }
    }
}
