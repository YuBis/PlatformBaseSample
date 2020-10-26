using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Platform_Android : PlatformBase<Platform_Android>
{
    void Start()
    {
    }

    void Update()
    {
    }
    public override void CheckOnTouches()
    {
        if( Input.touchCount == 0 )
            return;

        if( EventSystem.current.IsPointerOverGameObject(0) )
            return;

        var touchInfo = Input.touches[0];
        var touchPos = Camera.main.ScreenToWorldPoint(touchInfo.position);
        switch (touchInfo.phase)
        {
            case TouchPhase.Began :
            {
                OnTouchBegan(touchPos);
            } break;

            case TouchPhase.Moved :
            {
                OnTouchMoved(touchPos);
            } break;

            case TouchPhase.Ended :
            {
                OnTouchEnded(touchPos);
            } break;

            case TouchPhase.Stationary :
            {

            } break;

            case TouchPhase.Canceled :
            {

            } break;

            default : break;
        }
    }
}