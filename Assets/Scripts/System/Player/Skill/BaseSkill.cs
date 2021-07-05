using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : MonoBehaviour
{
    private bool _isActivated = true;


    [SerializeField]
    protected float coolTime = 1.0f;

    [SerializeField]
    protected float delayTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsExpiredCooltime()
    {
        return _isActivated;
    }

    protected virtual void OnActivation()
    {

    }

    protected virtual void OnStartAction()
    {

    }

    protected virtual void OnFinishDelayAction()
    {

    }


    public virtual void DoSkill()
    {
        if (_isActivated)
        {
            _isActivated = false;

            OnStartAction();

            StartCoroutine(WaitThenCallBack(delayTime, () =>
            {
                OnFinishDelayAction();
            }));

            StartCoroutine(WaitThenCallBack(coolTime, () =>
            {
                _isActivated = true;
                OnActivation();
            }));
        }
    }

    private IEnumerator WaitThenCallBack(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }
}
