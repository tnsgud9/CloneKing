using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    private GameObject _targetObject = null;
    private SpriteRenderer _spriteRenderer= null;
    private NicknameViewer _nickNameViewer = null;
    private PlayerColor _playerColor;

    private void InitializeComponents()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _nickNameViewer = GetComponentInChildren<NicknameViewer>();
    }

    public void Setup( GameObject go )
    {
        InitializeComponents();

        Debug.Assert(go != null);

        if ( go != null )
        {
            _targetObject = go;

            var playerController = go.GetComponent<PlayerController>();

            if( playerController != null)
            {
                _nickNameViewer.playerController = playerController;

                _playerColor =(PlayerColor)playerController.photonView.owner.CustomProperties["Color"];

                _spriteRenderer.color = _playerColor.PlayerColorToColor();
            }
        }
    }

    private void DriveIndicate()
    {
        const float expand_size = 0.03f;

        if( _targetObject != null)
        {
            // Position
            Vector3 targetPosition = _targetObject.gameObject.transform.position;
            Vector3 viewportPosition = Camera.main.WorldToViewportPoint(targetPosition);

            Debug.Log(viewportPosition);

            bool isVisible = viewportPosition.x <= 0.0f || viewportPosition.x >= 1.0f ||
                viewportPosition.y <= 0.0f || viewportPosition.y >= 1.0f;

            _spriteRenderer.enabled = isVisible;
            _nickNameViewer.enabled = isVisible;

            viewportPosition.x = Mathf.Clamp(viewportPosition.x, expand_size, 1.0f - expand_size);
            viewportPosition.y = Mathf.Clamp(viewportPosition.y, expand_size, 1.0f - expand_size);

            Vector3 worldPosition =  Camera.main.ViewportToWorldPoint(viewportPosition);

            transform.position = worldPosition;

            // Sprite Rotation
            var camera_center_position = Camera.main.ViewportToWorldPoint(new Vector3( 0.5f, 0.5f, 0.0f));

            Vector2 direction = targetPosition - camera_center_position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _spriteRenderer.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeComponents();
    }

    // Update is called once per frame
    void Update()
    {
        DriveIndicate();
    }
}
