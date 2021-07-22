using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterRoomPopup : MonoBehaviour
{
    public bool isCreateRoom = true;

    public InputField nickNameInputField = null;
    public InputField roomNameInputField = null;
    public Dropdown mapDropDown = null;
    public Text mainText = null;
    public Text submitText = null;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DriveAppear());
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetupPopup( bool isCreateRoomPopup)
    {
        isCreateRoom = isCreateRoomPopup;

        if (!isCreateRoomPopup)
        {
            mainText.text = "Join Room";
            submitText.text = "Join";

            mapDropDown.gameObject.SetActive(false);
            mapDropDown.enabled = false;
        }
    }

    public void OnSubmitButton()
    {
        NetworkManager.Instance.SetupNickName(nickNameInputField.text);

        Debug.Log(mapDropDown.itemText.text);
        if( isCreateRoom )
        {
            NetworkManager.Instance.CreateRoom(roomNameInputField.text, mapDropDown.options[mapDropDown.value].text);
        }
        else
        {
            NetworkManager.Instance.JoinRoom(roomNameInputField.text);
        }
    }

    public void Close()
    {
        Destroy(gameObject);
    }

    private IEnumerator DriveAppear()
    {
        float time = 0.0f;

        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;

        while( time <= 1.0f)
        {
            time += Time.deltaTime * 3.0f;

            transform.localScale = Vector3.Lerp(startScale, endScale, EasingFunction.EaseOutExpo(0.0f, 1.0f, Mathf.Clamp01(time)));

            yield return new WaitForEndOfFrame();
        }

        transform.localScale = endScale;
    }
}
