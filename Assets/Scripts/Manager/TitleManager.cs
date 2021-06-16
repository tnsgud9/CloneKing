using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Manager
{
    public class TitleManager : MonoBehaviour
    {
        private FadeSystem fadeSystem;
        public Text title;
        public AudioSource audioSource;
        public Image startButton;
        public Image exitButton;
        public Image fadeImage;
        
        void Start()
        {
            
            if (!this.GetComponent<FadeSystem>())
                fadeSystem = this.gameObject.AddComponent<FadeSystem>();
            
            audioSource = GetComponent<AudioSource>();
            
            audioSource.volume = 0f;
            title.color = new Color(1f, 1f, 1f, 0f);
            startButton.color = new Color(1f, 1f, 1f, 0f);
            exitButton.color = new Color(1f, 1f, 1f, 0f);

           StartCoroutine(IntroTimeline());
            
        }

        IEnumerator IntroTimeline()
        {
            fadeSystem.soundFadeIn(audioSource,10f);
            yield return new WaitForSeconds(1.5f);
            //fadeSystem.textFadeOut(title,5f);
            fadeSystem.textFadeOutRetro(title,0.1f,0.5f);
            yield return new WaitForSeconds(2f);
            fadeSystem.imageFadeOutRetro(startButton,0.1f,0.25f);
            //fadeSystem.textFadeOutRetro(startButton,0.1f,0.1f);
            fadeSystem.imageFadeOutRetro(exitButton,0.1f,0.25f);
            //fadeSystem.textFadeOutRetro(exitButton,0.1f,0.1f);
        }
        
        public void GameStart()
        {
            startButton.GetComponent<Button>().enabled = false;
            exitButton.GetComponent<Button>().enabled = false;
            StartCoroutine(GameStartTimeline());
        }

        IEnumerator GameStartTimeline()
        {
            fadeSystem.soundFadeOut(audioSource,3f);
            fadeSystem.textFadeInRetro(title,0.1f,0.3f);
            fadeSystem.imageFadeInRetro(fadeImage,0.1f,0.3f);
            fadeSystem.imageFadeInRetro(startButton,0.1f,0.25f);
            fadeSystem.imageFadeInRetro(exitButton,0.1f,0.25f);
            yield return new WaitForSeconds(3.5f);
            SceneManager.LoadScene("Map1");
        }

        public void ExitButton()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
        }


    }
    
    
    
}
