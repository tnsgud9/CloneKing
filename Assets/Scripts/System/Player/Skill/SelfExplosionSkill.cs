using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfExplosionSkill : BaseSkill
{
    public float explosionWaitForTime = 5.0f;
    public float redColorTwinkleSpeed = 4.0f;

    private SpriteRenderer _spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        coolTime = 10.0f;
        delayTime = 0.0f;

        InitializeComponents();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeComponents()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

    }


    protected override void OnStartAction()
    {
        base.OnStartAction();

        StartCoroutine(DriveExplosionEffects());
    }

    protected override void OnFinishDelayAction()
    {
        base.OnFinishDelayAction();
    }

    protected override void OnActivation()
    {
        base.OnActivation();
    }


    private IEnumerator DriveExplosionEffects()
    {
        float duration = 0.0f;

        float colorDeltaFlip = 1.0f;
        float currentRedWidget = 0.0f;

        while (duration < explosionWaitForTime)
        {
            currentRedWidget += redColorTwinkleSpeed * Time.deltaTime * colorDeltaFlip;

            if( currentRedWidget <= 0.0f)
            {
                currentRedWidget = 0.0f;
                colorDeltaFlip *= -1.0f;
            }


            if (currentRedWidget >= 1.0f)
            {
                currentRedWidget = 1.0f;
                colorDeltaFlip *= -1.0f;
            }

            _spriteRenderer.color = new Color(1.0f, 1.0f - currentRedWidget, 1.0f - currentRedWidget, 1.0f);
            
            duration += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        ExplodeSelf();
    }

    private void ExplodeSelf()
    {

    }
}
