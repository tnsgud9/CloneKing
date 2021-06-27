using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LobbyManager : Manager.Singleton<LobbyManager>
{
    private string _room_Name = string.Empty;

    public InputField roomNameInputField;

    public Text messageBoxText;
    public Button joinButton;
    public Button createRoomButton;

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
        joinButton.onClick.AddListener(OnClickedJoinButton);
        createRoomButton.onClick.AddListener(OnClickedCreateRoomButton);
    }

    private void OnClickedJoinButton()
    {
        NetworkManager.Instance.JoinRoom(roomNameInputField.text);
    }

    private void OnClickedCreateRoomButton()
    {
        NetworkManager.Instance.CreateRoom(roomNameInputField.text);
    }

}
