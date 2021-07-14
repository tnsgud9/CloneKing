using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRankingItem : MonoBehaviour
{
    public Text RankText;
    public Text NameText;

    private PhotonPlayer _targetPlayer;

    public void SetupPlayer( PhotonPlayer player)
    {
        _targetPlayer = player;

        if( player == null)
        {
            RankText.text = "1";
            NameText.text = "¿Ã∏ß";
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTexts();
    }

    void UpdateTexts()
    {
        if (_targetPlayer != null)
        {
            if (RankText != null)
            {
                int rank = (int)_targetPlayer.CustomProperties["Rank"];
                RankText.text = rank.ToString();
            }

            if (NameText != null)
            {
                NameText.text = _targetPlayer.NickName;
            }
        }
    }
}
