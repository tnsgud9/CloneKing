using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon;

public class LobbyManager : Photon.PunBehaviour
{
    private string _gameVersion = "Temp";
    private string _room_Name = string.Empty;

    public Text RoomNameText;
    public Button RoomJoinButton;

    public Text MessageBoxText;
    public Button JoinButton;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.gameVersion = _gameVersion;

        PhotonNetwork.ConnectUsingSettings(_gameVersion);

        UpdateNetworkWidget();
    }

    public override void OnConnectedToMaster()
    {
        UpdateNetworkWidget();

        base.OnConnectedToMaster();
    }

    public override void OnDisconnectedFromPhoton()
    {
        UpdateNetworkWidget();

        base.OnDisconnectedFromPhoton();
    }

    private void UpdateNetworkWidget()
    {
        string connection_msg = PhotonNetwork.connectionState.ToString();
        bool button_enable = true;

        switch(PhotonNetwork.connectionState)
        {
            case ConnectionState.Connecting:
            case ConnectionState.Disconnected:
            case ConnectionState.Disconnecting:
                button_enable = false;
                break;
        }

        MessageBoxText.text = connection_msg;
        JoinButton.interactable = button_enable;
        JoinButton.onClick.AddListener(JoinLobby);
    }


    private void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    private void JoinRoom( string _room_name)
    {
        PhotonNetwork.JoinRoom(_room_name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
