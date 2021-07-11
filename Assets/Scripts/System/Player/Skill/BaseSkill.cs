using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : MonoBehaviour
{
    private bool _isActivated = true;


    public GameObject gauge;
    private Animation _gagueAnimation;
    protected PlayerController _playerController;

    [SerializeField]
    protected float coolTime = 1.0f;

    [SerializeField]
    protected float delayTime = 1.0f;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        Debug.Log("Init");
        gauge = gameObject.transform.Find("Reload Bar").gameObject;

        _gagueAnimation = gauge.transform.GetChild(0).transform.GetChild(1).GetComponent<Animation>();

        gauge.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void BindPlayerController( PlayerController playerController)
    {
        _playerController = playerController;
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

            gauge.SetActive(true);
            _gagueAnimation["gauge_refill"].speed = (10.0f / coolTime);
            _gagueAnimation.Play();

            OnStartAction();

            StartCoroutine(WaitThenCallBack(delayTime, () =>
            {
                OnFinishDelayAction();
            }));

            StartCoroutine(WaitThenCallBack(coolTime, () =>
            {
                _isActivated = true;
                gauge.SetActive(false);
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
