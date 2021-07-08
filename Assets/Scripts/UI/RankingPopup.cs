using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RankingPopup : MonoBehaviour
{
    public ScrollRect ScrollRect;
    private string _rankingItemPath = "Prefabs/UI/PlayerRankingItem";

    // Start is called before the first frame update
    void Start()
    {
        var parent_transform = ScrollRect.content.transform;

        foreach ( var player in Manager.GameManager.Instance.GetPlayers())
        {
            GameObject go = Instantiate(Resources.Load(_rankingItemPath) as GameObject);
            go.transform.SetParent(parent_transform, false);

            var rankingItem = go.GetComponent<PlayerRankingItem>();

            if (rankingItem != null)
            {
                rankingItem.SetupPlayer(player.owner);
            }
        }

    }

    public void OnClickExit()
    {
        NetworkManager.Instance.ExitRoom();

        SceneManager.LoadScene("Lobby");
    }
}
