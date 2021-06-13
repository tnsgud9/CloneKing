using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    private FadeSystem fadeSystem;
    public Image fadeImage;

    public static MapManager instance;
    public GameObject player;
    
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
    
    void Start()
    {
        if (!GetComponent<FadeSystem>()) fadeSystem = this.gameObject.AddComponent<FadeSystem>();
        fadeImage.color = new Color(0f, 0f, 0f, 1f);
        fadeSystem.imageFadeInRetro(fadeImage,0.1f,0.15f); 
        StartCoroutine(
        waitThenCallback(1f, () =>
        {
            Debug.Log("Player 생성"); 
            Instantiate(player);
        }));

    }

    private IEnumerator waitThenCallback(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }

    public void GotoTitle()
    {
        fadeSystem.imageFadeOutRetro(fadeImage, 0.1f, 0.25f);
        StartCoroutine(waitThenCallback(3f, () =>
        {
            SceneManager.LoadScene("Title");
        }));
    }

}
