using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LobbyManager : Manager.Singleton<LobbyManager>
{
    private string _roomName = "Temp";
    public GameObject enterRoomPopup = null;
    public InputField nickNameInputField;

    public Canvas mainCanvas;
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
        joinButton.onClick.RemoveAllListeners();
        createRoomButton.onClick.RemoveAllListeners();

        joinButton.onClick.AddListener(OnClickedJoinButton);
        createRoomButton.onClick.AddListener(OnClickedCreateRoomButton);
    }

    private void OnClickedJoinButton()
    {
        NetworkManager.Instance.SetupNickName(nickNameInputField.text);

        GameObject go = Instantiate(enterRoomPopup);
        go.transform.SetParent(mainCanvas.transform, false);
        go.GetComponent<EnterRoomPopup>().SetupPopup(false);
    }

    private void OnClickedCreateRoomButton()
    {
        NetworkManager.Instance.SetupNickName(nickNameInputField.text);

        GameObject go = Instantiate(enterRoomPopup);
        go.transform.SetParent(mainCanvas.transform, false);
        go.GetComponent<EnterRoomPopup>().SetupPopup(true);

        /*
        NetworkManager.Instance.CreateRoom(_roomName);
        */
    }

}
