using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LobbyManager : Manager.Singleton<LobbyManager>
{
    public GameObject enterRoomPopupOrigin = null;


    public Dropdown skillDropDown = null;
    public Canvas mainCanvas;
    public Text messageBoxText;
    public Button joinButton;
    public Button createRoomButton;

    private GameObject _roomPopup = null;

    public void Start()
    {
        if( skillDropDown != null)
        {
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

            foreach (var skillValue in typeof(SkillType).GetEnumValues())
            {
                options.Add(new Dropdown.OptionData(((SkillType)skillValue).ToString()));
            }

            skillDropDown.AddOptions(options);
        }
    }

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
        skillDropDown.enabled = button_enable;

        // Bind UI Event 
        joinButton.onClick.RemoveAllListeners();
        createRoomButton.onClick.RemoveAllListeners();

        joinButton.onClick.AddListener(OnClickedJoinButton);
        createRoomButton.onClick.AddListener(OnClickedCreateRoomButton);
    }

    public void OnClickedExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif

    }

    private void OnClickedJoinButton()
    {        
        ShowEnterRoomPopup(false);
    }

    private void OnClickedCreateRoomButton()
    {
        ShowEnterRoomPopup(true);
    }

    private void ShowEnterRoomPopup( bool is_create_room)
    {
        if (skillDropDown != null)
        {
            string text = skillDropDown.options[skillDropDown.value].text;
            SkillType type =(SkillType)System.Enum.Parse(typeof(SkillType), text);

            PhotonNetwork.player.CustomProperties["SkillType"] = type;
        }

        if (_roomPopup == null)
        {
            _roomPopup = Instantiate(enterRoomPopupOrigin);
            _roomPopup.transform.SetParent(mainCanvas.transform, false);
            _roomPopup.GetComponent<EnterRoomPopup>().SetupPopup(is_create_room);
        }
    }

}
