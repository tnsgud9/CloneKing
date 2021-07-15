using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CommonFunctionsUI : MonoBehaviour
{
    public bool AlphaAppear = false;
    public bool ExpandAppear = true;
    public bool TranslateAppear = false;
    public bool UVScrolling = false;

    public Vector3 startPoisition = Vector3.zero;
    public Vector3 endPosition = Vector3.one;
        
    public float AppearTime = 1.0f;
    public float AppearDelay = 0.0f;

    private Image _image = null;

    public void Update()
    {
        if( UVScrolling)
        {
            var mat = GetComponent<Image>().material;

            var offset =mat.mainTextureOffset;

            offset.x = 0.0f;
            offset.y += Time.deltaTime * 0.01f;

            mat.mainTextureOffset = offset;
        }
    }

    public void Start()
    {


        if (ExpandAppear)
        {
            gameObject.transform.localScale = Vector3.zero;

            StartCoroutine(DriveTimer(AppearDelay, AppearTime, EasingFunction.Ease.EaseOutExpo,
            (float factor) => { gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, factor); } ));
        }

        if(AlphaAppear)
        {
            _image = GetComponent<Image>();
            _image.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

            StartCoroutine(DriveTimer(AppearDelay, AppearTime, EasingFunction.Ease.EaseOutExpo,
            (float factor) => { _image.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Lerp(0.0f, 1.0f, factor)); } ));
        }

        if(TranslateAppear)
        {
            gameObject.transform.localPosition= startPoisition;

            StartCoroutine(DriveTimer(AppearDelay, AppearTime, EasingFunction.Ease.EaseInOutElastic,
            (float factor) => { gameObject.transform.localPosition = Vector3.Lerp(startPoisition, endPosition, factor); }));
        }
    }

    IEnumerator DriveTimer( float delayTime, float time, EasingFunction.Ease easingType, Action<float> callBack)
    {
        if (delayTime > 0.0f)
        {
            yield return new WaitForSeconds(delayTime);
        }

        float elapsedTime = 0.0f;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            float factor = EasingFunction.GetEasingFunction(easingType).Invoke(0.0f, 1.0f, Mathf.Clamp01(elapsedTime / time));

            if (callBack != null)
            {
                callBack.Invoke(factor);
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void ChangeScene( string name)
    {
        if( PhotonNetwork.inRoom)
        {
            NetworkManager.Instance.ExitRoom();
        }

        SceneManager.LoadScene(name);
    }

    public void ClosePopup()
    {
        Destroy(this.gameObject);
    }
}
