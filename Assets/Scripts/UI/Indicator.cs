using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    private GameObject _targetObject = null;
    private SpriteRenderer _spriteRenderer= null;


    private void InitializeComponents()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Setup( GameObject go )
    {
        Debug.Assert(go != null);

        if ( go != null )
        {
            _targetObject = go;
        }
    }

    private void DriveIndicate()
    {
        const float expand_size = 0.01f;

        if( _targetObject != null)
        {
            Vector3 targetPosition = _targetObject.gameObject.transform.position;
            Vector3 viewportPosition = Camera.main.WorldToViewportPoint(targetPosition);

            Debug.Log(viewportPosition);

            bool isVisible = viewportPosition.x <= 0.0f || viewportPosition.x >= Screen.width ||
                viewportPosition.y <= 0.0f || viewportPosition.y >= Screen.height;

            _spriteRenderer.enabled = isVisible;

            viewportPosition.x = Mathf.Clamp(viewportPosition.x, expand_size, 1.0f - expand_size);
            viewportPosition.y = Mathf.Clamp(viewportPosition.y, expand_size, 1.0f - expand_size);

            Vector3 worldPosition =  Camera.main.ViewportToWorldPoint(viewportPosition);

            transform.position = worldPosition;
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
