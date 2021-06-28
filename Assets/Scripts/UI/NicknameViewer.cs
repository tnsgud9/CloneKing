using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NicknameViewer : MonoBehaviour
{
    private TextMesh _textMesh;

    [SerializeField]
    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        InitializeComponents();
    }

    void InitializeComponents()
    {
        _textMesh = GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_textMesh != null && playerController != null)
        {
            _textMesh.text = playerController.photonView.owner.NickName;
        }
    }
}
