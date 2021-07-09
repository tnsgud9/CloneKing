using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelector : MonoBehaviour
{
    public string itemEntityPath = "Prefabs/UI/ColorSelectEntity";

    [SerializeField]
    public string enumName = "PlayerColor";

    private ScrollRect _scrollRect;
    private List<BaseSelectItem> _items;


    // Start is called before the first frame update
    void Start()
    {
        InitializeComponents();
        InitializeSelectItems();
    }


    public void OnSelectedItem(BaseSelectItem selectedItem )
    {
        foreach( var item in _items)
        {
            item.SetSelection(false);
        }

        selectedItem.SetSelection(true);
    }

    private void InitializeSelectItems()
    {
        _items = new List<BaseSelectItem>();

        _items.Clear();

        var parent_transform = _scrollRect.content.transform;
        
        foreach ( var enum_value in System.Type.GetType(enumName).GetEnumValues())
        {
            GameObject go = Instantiate(Resources.Load(itemEntityPath) as GameObject);
            go.transform.SetParent(parent_transform, false);

            var baseSelectItem = go.GetComponent<BaseSelectItem>();

            if (baseSelectItem != null)
            {
                baseSelectItem.Initialize((int)enum_value);
                baseSelectItem.BindSelector(this);

                _items.Add(baseSelectItem);
            }
        }

        OnSelectedItem(_items[0]);
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
