using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


interface ISkill
{
    int coolTime { get; set; }
    void action();
}

public class PlayerSkill : MonoBehaviour
{
    
    public SkillTypes type;
    private ISkill skillHandler;
    private void Start()
    {
        switch (type)
        {
            case SkillTypes.None:
                break;
            case SkillTypes.PushHand:
                skillHandler = GetComponent<PushHandSkill>();
                if (skillHandler == null)
                    skillHandler = gameObject.AddComponent<PushHandSkill>();
                break;
            
            // If you have a new skill, you can write it down below.
        }
    }

    public void skillAction()
    {
        skillHandler.action();
    }
}
