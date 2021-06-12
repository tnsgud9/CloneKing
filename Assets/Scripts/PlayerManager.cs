using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    
    void Start()
    {
        GameManager.instance.AddPlayer(this.gameObject);
    }

}
