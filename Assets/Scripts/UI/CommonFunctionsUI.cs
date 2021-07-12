using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CommonFunctionsUI : MonoBehaviour
{
    public bool ExpandAppear = true;

    public void Start()
    {
        if( ExpandAppear)
        {
            StartCoroutine(DriveAppear());
        }
    }

    IEnumerator DriveAppear()
    {
        float time = 0.0f;

        while( time < 1.0f)
        {
            time += Time.deltaTime;

            gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, EasingFunction.EaseOutExpo(0.0f, 1.0f, Mathf.Clamp01(time)));

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
