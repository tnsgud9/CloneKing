using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TeleportCloud : SynchronizedObject
{
    private const float moveSpeed = 4.0f;

    private float _remainLifeTime = 1.5f;

    private Vector3 _searchingPosition = Vector3.zero;
    private Vector3 _moveVelocity = Vector3.zero;
    private MoveState _currentMoveState = MoveState.ReadyToMove;

    private enum MoveState
    {
        ReadyToMove,
        ToOwner,
        ToDestination,
        ToBeDestroy,
    }

    private void Awake()
    {
        lifeTime = 15.0f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (photonView.isMine)
        {
            DriveMovement();
        }
    }

    [PunRPC]
    void Attach( int photonID, Vector3 serachingPosition)
    {
        var photonView = PhotonView.Find(photonID);
        var controller = photonView.gameObject.GetComponent<PlayerController>();

        if(controller != null)
        {
            controller.gameObject.transform.SetParent(transform);
            controller.gameObject.transform.localPosition = new Vector3(0.0f, 0.15f, 0.0f);

            _searchingPosition = serachingPosition;
        }
    }

    [PunRPC]
    void Detach(int photonID )
    {
        var photonView = PhotonView.Find(photonID);
        var controller = photonView.gameObject.GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.GetComponent<TeleportSkill>().ReleaseHolding();
            gameObject.transform.DetachChildren();
        }
    }


    private void ChangeMoveState( MoveState state)
    {
        _currentMoveState = state;
    }

    private void DriveMovement()
    {
        switch( _currentMoveState)
        {
            case MoveState.ReadyToMove:
                ChangeMoveState(MoveState.ToOwner);
                break;

            case MoveState.ToOwner:
                if( MoveToPosition(_owner.transform.position + Vector3.down * 0.05f))
                {
                    SerachingProperPosition();

                    photonView.RPC("Attach", PhotonTargets.All, _owner.photonView.viewID, _searchingPosition);

                    ChangeMoveState(MoveState.ToDestination);
                }
                break;

            case MoveState.ToDestination:
                if( MoveToPosition(_searchingPosition) )
                {
                    photonView.RPC("Detach", PhotonTargets.All, _owner.photonView.viewID);

                    ChangeMoveState(MoveState.ToBeDestroy);
                }
                break;

            case MoveState.ToBeDestroy:
                DriveDestory();
                break;
        }
    }

    private void SerachingProperPosition()
    {
        const float heightIncreasement = 4.0f;
        const float measureUnit = 0.1f;

        _searchingPosition = Vector3.zero;
        Vector2 serachingCenterPosition = new Vector2(0.0f, transform.position.y + heightIncreasement);
        int layerIndex = 1 << LayerMask.NameToLayer("TileMap");

        while( serachingCenterPosition.y > transform.position.y )
        {
            RaycastHit2D hit = Physics2D.BoxCast(serachingCenterPosition, new Vector2(2.0f, measureUnit), 0.0f, Vector2.down, measureUnit, layerIndex);
            if (hit.collider != null && hit.normal.y > 0.95f && hit.distance > 0.0f )
            {
                _searchingPosition = new Vector3(hit.point.x, hit.point.y + 0.25f, 0.0f);
                break;
            }

            serachingCenterPosition.y -= measureUnit;
        }
    }


    private bool MoveToPosition( Vector3 position)
    {
        Vector3 moveDirection = position - transform.position;
        moveDirection.Normalize();

        _moveVelocity += moveDirection * moveSpeed * Time.deltaTime * 2.0f;

        if( _moveVelocity.sqrMagnitude > moveSpeed)
        {
            _moveVelocity = (_moveVelocity + moveDirection * 0.1f).normalized * moveSpeed;
        }


        if( (position - transform.position).sqrMagnitude.NearlyEquals( 0.0f, 0.01f))
        {
            return true;
        }
        else
        {
            transform.position += _moveVelocity * Time.deltaTime;
        }

        return false;
    }

    private void DriveDestory()
    {
        _remainLifeTime -= Time.deltaTime;
        if( _remainLifeTime <=0.0f)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
