using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    
    private PlayerMove _playerMove;
    private PlayerJump _playerJump;
    private PlayerSkill _playerSkill;
    
    void Start()
    {
        InitializeComponents();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void InitializeComponents()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerJump = GetComponent<PlayerJump>();
        _playerSkill = GetComponent<PlayerSkill>();
    }
}
