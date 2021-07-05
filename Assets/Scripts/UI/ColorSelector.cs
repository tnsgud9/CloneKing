using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelector : MonoBehaviour
{
    private GameObject _colorEntry;
    private string _colorEntryPath = "Prefabs/UI/ColorSelectEntity";
    private ScrollRect _scrollRect;
    private List<ColorSelectEnity> _colorSelectEntries;

    // Start is called before the first frame update
    void Start()
    {
        InitializeComponents();
        InitializeColorEntries();
    }

    public void OnSelectedColor( ColorSelectEnity entity )
    {
        foreach( var entry in _colorSelectEntries)
        {
            entry.SetSelection(false);
        }
        entity.SetSelection(true);
    }

    private void InitializeColorEntries()
    {
        _colorSelectEntries = new List<ColorSelectEnity>();

        _colorSelectEntries.Clear();

        var parent_transform = _scrollRect.content.transform;

        for (int i = 0; i < (int)PlayerColor.Max; ++i)
        {
            PlayerColor color = (PlayerColor)i;

            GameObject go = Instantiate(Resources.Load(_colorEntryPath) as GameObject);
            go.transform.SetParent(parent_transform);

            var colorSelectEntity = go.GetComponent<ColorSelectEnity>();

            if (colorSelectEntity != null)
            {
                colorSelectEntity.SetupColor(this, color);
                _colorSelectEntries.Add(colorSelectEntity);
            }
        }

        OnSelectedColor(_colorSelectEntries[0]);
    }

    private void InitializeComponents()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
