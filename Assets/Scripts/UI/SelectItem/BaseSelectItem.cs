using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BaseSelectItem : MonoBehaviour
{
    private string _propertyKey;

    protected bool _isSelection = false;

    private Button _button;
    private Action<BaseSelectItem> clickAction;


    private void Awake()
    {
    }

    public virtual void Initialize( int enumIndex)
    {

    }

    public virtual void BindSelector(ItemSelector selector)
    {
        _button = GetComponent<Button>();

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(OnClickButton);

        clickAction = selector.OnSelectedItem;
    }

    public void OnClickButton()
    {
        clickAction.Invoke(this);
    }

    public virtual void OnSelectedItem()
    {

    }

    public void SetSelection(bool isSelection)
    {
        if (PhotonNetwork.connected)
        {
            _isSelection = isSelection;

            OnSelectedItem();
        }
    }
}
