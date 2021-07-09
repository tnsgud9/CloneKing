using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterRoomPopup : MonoBehaviour
{
    public bool isCreateRoom = true;

    public InputField roomNameInputField = null;
    public Dropdown mapDropDown = null;
    public Text mainText = null;
    public Text submitText = null;


    // Start is called before the first frame update
    void Start()
    {
        
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
}
