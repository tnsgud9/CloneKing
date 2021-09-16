using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    //public
    public GameObject camera; 
    
    //private
    private PlayerMove _playerMove;
    private PlayerJump _playerJump;
    private PlayerSkill _playerSkill;
    
    void Start()
    {
        InitializeComponents();
        
    }

    void Update()
    {
        JumpInput();
        MoveInput();
        SkillInput();
    }
    
    private void InitializeComponents()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerJump = GetComponent<PlayerJump>();
        _playerSkill = GetComponent<PlayerSkill>();
        
    }



    #region PlayerInputs
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
            _playerMove.MoveEvent(Input.GetAxisRaw("Horizontal"));
    }
    
    private void SkillInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Debug.Log("Actived Player Skills");
            /*
            if (_playerSkill.IsExpiredCooltime())
            {
                _playerSkill.DoSkill();
                photonView.RPC("RPC_DoSkill", PhotonTargets.All);
            }
            */
        }
    }
    #endregion

}
