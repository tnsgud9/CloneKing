using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerText : MonoBehaviour
{
    protected Text _text = null;
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if( _text != null)
        {
            int timeCount = Manager.GameManager.Instance.timeCount;

            int hour = 0, minute = 0, second = 0;

            hour = (timeCount % (60 * 60 * 24)) / (60 * 60);
            minute = (timeCount % (60 * 60)) / (60);
            second = timeCount % (60);

            _text.text = string.Format("{0}:{1}:{2}", hour, minute, second);
        }
    }
}
