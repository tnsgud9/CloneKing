using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEmotionControl : MonoBehaviour
{
    private bool _canActive = false;
    private float _coolTime = 1.5f;


    // Start is called before the first frame update
    void Start()
    {
        _canActive = true;
    }

    public bool IsExpiredCoolTime() { return _canActive; }

    public void DoEmote( PlayerController playerController, EmotionType emotionType)
    {
        if( IsExpiredCoolTime())
        {
            _canActive = false;
            playerController.photonView.RPC("RPC_Emote", PhotonTargets.All, emotionType);
            StartCoroutine(CoolTimeCoroutine());

        }
    }

    private IEnumerator CoolTimeCoroutine()
    {
        yield return new WaitForSeconds(_coolTime);
        _canActive = true;
    }
}
