using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NicknameViewer : MonoBehaviour
{
    private TextMesh _textMesh;
    private MeshRenderer _meshRenderer;

    [SerializeField]
    public PlayerController playerController;


    public void OnEnable()
    {
        if( _meshRenderer != null)
            _meshRenderer.enabled = true;
    }

    public void OnDisable()
    {
        if (_meshRenderer != null)
            _meshRenderer.enabled = false;

    }
    // Start is called before the first frame update
    void Awake()
    {
        InitializeComponents();
    }

    void InitializeComponents()
    {
        _textMesh = GetComponent<TextMesh>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_textMesh != null && playerController != null)
        {
            if (PhotonNetwork.offlineMode)
            {
                _textMesh.text = "Player";
            }
            else
            {
                string nameText = playerController.photonView.owner.NickName;

                int rank = -1;
                if (playerController.photonView.TryGetValueToInt("Rank", out rank))
                {
                    nameText = rank.ToString() + ". " + nameText;
                }

                _textMesh.text = nameText;

                var color = (PlayerColor)playerController.photonView.owner.CustomProperties["Color"];
                _textMesh.color = color.PlayerColorToColor();
            }
        }
    }
}
