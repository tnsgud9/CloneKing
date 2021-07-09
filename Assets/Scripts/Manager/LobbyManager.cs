using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LobbyManager : Manager.Singleton<LobbyManager>
{
    public GameObject enterRoomPopupOrigin = null;
    public InputField nickNameInputField;

    public Canvas mainCanvas;
    public Text messageBoxText;
    public Button joinButton;
    public Button createRoomButton;

    private GameObject _roomPopup = null;

    public void UpdateConnectionWidgets()
    {
        string connection_msg = PhotonNetwork.connectionState.ToString();
        bool button_enable = true;

        switch (PhotonNetwork.connectionState)
        {
            case ConnectionState.Connecting:
            case ConnectionState.Disconnected:
            case ConnectionState.Disconnecting:
                button_enable = false;
                break;
        }

        // UI Update
        messageBoxText.text = connection_msg;
        joinButton.interactable = button_enable;
        createRoomButton.interactable = button_enable;

        // Bind UI Event 
        joinButton.onClick.RemoveAllListeners();
        createRoomButton.onClick.RemoveAllListeners();

        joinButton.onClick.AddListener(OnClickedJoinButton);
        createRoomButton.onClick.AddListener(OnClickedCreateRoomButton);
    }

    private void OnClickedJoinButton()
    {
        NetworkManager.Instance.SetupNickName(nickNameInputField.text);

        ShowEnterRoomPopup(false);
    }

    private void OnClickedCreateRoomButton()
    {
        NetworkManager.Instance.SetupNickName(nickNameInputField.text);

        ShowEnterRoomPopup( true );

    }

    private void ShowEnterRoomPopup( bool is_create_room)
    {
        if (_roomPopup == null)
        {
            _roomPopup = Instantiate(enterRoomPopupOrigin);
            _roomPopup.transform.SetParent(mainCanvas.transform, false);
            _roomPopup.GetComponent<EnterRoomPopup>().SetupPopup(is_create_room);
        }
    }

}
