using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeSystem : MonoBehaviour
{

    #region Sound Fade

    
    public void soundFadeOut(AudioSource audioSource, float fadeTime)
    {
        StartCoroutine(soundFadeOutPlay(audioSource, fadeTime));
    }
    private IEnumerator soundFadeOutPlay(AudioSource audioSource, float fadeTime)
    {
        while(audioSource.volume >= 0f){
            audioSource.volume -= Time.deltaTime / fadeTime;
            yield return null;
        }
        audioSource.volume = 0f;
    }
    public void soundFadeIn(AudioSource audioSource, float fadeTime)
    {
        StartCoroutine(soundFadeInPlay(audioSource, fadeTime));
    }
    private IEnumerator soundFadeInPlay(AudioSource audioSource, float fadeTime)
    {
        while(audioSource.volume <= 1f){
            audioSource.volume += Time.deltaTime / fadeTime;
            yield return null;
        }
        audioSource.volume = 1f;
    }

    #endregion



    #region Sprite Fade
    public void spriteFadeIn(SpriteRenderer sprite, float fadeTime)
    {
        StartCoroutine(spriteFadeInPlay(sprite, fadeTime));
    }
    private IEnumerator spriteFadeInPlay(SpriteRenderer sprite, float fadeTime)
    {
        Color color = sprite.color;
        while(color.a <= 1f){
            color.a += Time.deltaTime / fadeTime;
            sprite.color = color;
            yield return null;
        }
        sprite.color = new Color(sprite.color.r,sprite.color.g,sprite.color.b,1f);
    }
    
    public void spriteFadeOut(SpriteRenderer sprite, float fadeTime)
    {
        StartCoroutine(spriteFadeInPlay(sprite, fadeTime));
    }
    private IEnumerator spriteFadeOutPlay(SpriteRenderer sprite, float fadeTime)
    {
        Color color = sprite.color;
        while(color.a >= 0f){
            color.a -= Time.deltaTime / fadeTime;
            sprite.color = color;
            yield return null;
        }
        sprite.color = new Color(sprite.color.r,sprite.color.g,sprite.color.b,0f);
    }
    #endregion
    
    #region Image Fade
    public void imageFadeIn(Image sprite, float fadeTime)
    {
        StartCoroutine(imageFadeInPlay(sprite, fadeTime));
    }
    private IEnumerator imageFadeInPlay(Image image, float fadeTime)
    {
        Color color = image.color;
        while(color.a <= 1f){
            color.a += Time.deltaTime / fadeTime;
            image.color = color;
            yield return null;
        }
        image.color = new Color(image.color.r,image.color.g,image.color.b,1f);
    }
    
    public void imageFadeOut(Image image, float fadeTime)
    {
        StartCoroutine(imageFadeInPlay(image, fadeTime));
    }
    private IEnumerator imageFadeOutPlay(Image image, float fadeTime)
    {
        Color color = image.color;
        while(color.a >= 0f){
            color.a -= Time.deltaTime / fadeTime;
            image.color = color;
            yield return null;
        }
        image.color = new Color(image.color.r,image.color.g,image.color.b,0f);
    }
    
    
    public void imageFadeInRetro(Image image, float fadeSize,float fadeDelay)
    {
        StartCoroutine(imageFadeOutPlay(image, fadeSize,fadeDelay));
    }
    private IEnumerator imageFadeOutPlay(Image image, float fadeSize,float fadeDelay)
    {
        Color color = image.color;
        while(color.a >= 0f){
            color.a -= fadeSize;
            image.color = color;
            yield return new WaitForSeconds(fadeDelay);
        }
        image.color = new Color(image.color.r,image.color.g,image.color.b,0f);
    }
    
    public void imageFadeOutRetro(Image image, float fadeSize,float fadeDelay)
    {
        StartCoroutine(imageFadeInPlay(image, fadeSize,fadeDelay));
    }
    private IEnumerator imageFadeInPlay(Image image, float fadeSize,float fadeDelay)
    {
        Color color = image.color;
        while(color.a <= 1f){
            color.a += fadeSize;
            image.color = color;
            yield return new WaitForSeconds(fadeDelay);
        }
        image.color = new Color(image.color.r,image.color.g,image.color.b,1f);
    }
    
    #endregion
    
    
    
    #region Text Fade
    public void textFadeIn(Text text, float fadeTime)
    {
        StartCoroutine(textFadeInPlay(text, fadeTime));
    }
    private IEnumerator textFadeInPlay(Text text, float fadeTime)
    {
        Color color = text.color;
        while(color.a <= 1f){
            color.a += Time.deltaTime / fadeTime;
            text.color = color;
            yield return null;
        }
        text.color = new Color(text.color.r,text.color.g,text.color.b,1f);
    }
    public void textFadeOut(Text text, float fadeTime)
    {
        StartCoroutine(textFadeInPlay(text, fadeTime));
    }
    private IEnumerator textFadeOutPlay(Text text, float fadeTime)
    {
        Color color = text.color;
        while(color.a >= 0f){
            color.a -= Time.deltaTime / fadeTime;
            text.color = color;
            yield return null;
        }
        text.color = new Color(text.color.r,text.color.g,text.color.b,0f);
    }
    
    
    public void textFadeInRetro(Text text, float fadeSize,float fadeDelay)
    {
        StartCoroutine(textFadeOutPlay(text, fadeSize,fadeDelay));
    }
    private IEnumerator textFadeOutPlay(Text text, float fadeSize,float fadeDelay)
    {
        Color color = text.color;
        while(color.a >= 0f){
            color.a -= fadeSize;
            text.color = color;
            yield return new WaitForSeconds(fadeDelay);
        }
        text.color = new Color(text.color.r,text.color.g,text.color.b,0f);
    }
    
    public void textFadeOutRetro(Text text, float fadeSize,float fadeDelay)
    {
        StartCoroutine(textFadeInPlay(text, fadeSize,fadeDelay));
    }
    private IEnumerator textFadeInPlay(Text text, float fadeSize,float fadeDelay)
    {
        Color color = text.color;
        while(color.a <= 1f){
            color.a += fadeSize;
            text.color = color;
            yield return new WaitForSeconds(fadeDelay);
        }
        text.color = new Color(text.color.r,text.color.g,text.color.b,1f);
    }
    #endregion


    
    
}
