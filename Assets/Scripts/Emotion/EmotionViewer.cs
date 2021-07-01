using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EmotionViewer : MonoBehaviour
{
    private EmotionType _emotionType;
    private SpriteRenderer _spriteRenderer;
    private IEnumerator _lifeTimeTimer;

    private float _durationTime;
    private float _elapsedTime;

    public void SetupEmotion( EmotionType type, float duration)
    {
        _durationTime = duration;
        _elapsedTime = 0.0f;

        _emotionType = type;

        transform.localScale = Vector3.zero;
        switch(_emotionType)
        {
            case EmotionType.ThumbsDown:
                break;

            case EmotionType.ThumbsUp:
                break;
        }
    }

    private void InitializeComponents()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeComponents();
        StartCoroutine(LifeTimeCoroutine());
    }

    private IEnumerator LifeTimeCoroutine()
    {
        const float half = 0.5f;

        while( _elapsedTime <= _durationTime)
        {
            _elapsedTime += Time.deltaTime;

            float factor = Mathf.Clamp01( _elapsedTime / ( _durationTime * half));

            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, factor);
            
            if (_spriteRenderer != null)
            {
                var sprite_color = _spriteRenderer.color;
                sprite_color.a = Mathf.Clamp01(_durationTime - _elapsedTime);

                _spriteRenderer.color = sprite_color;
            }

            yield return null;

        }

        Destroy(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
