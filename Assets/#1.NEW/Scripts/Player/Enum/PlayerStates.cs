using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillTypes
{
    None,
    PushHand,
    
}


// If all Enum is used, it is supported in the form of a class.
public class PlayerStates
{
    public SkillTypes SkillType { get; set; }
    
}
