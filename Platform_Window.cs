using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Platform_Window : PlatformBase<Platform_Window>
{
    void Start()
    {
    }

    void Update()
    {
    }

    public override void CheckOnTouches()
    {
        if( EventSystem.current.IsPointerOverGameObject() )
            return;

        // var touchPos = CameraController.GetInstance.m_camera.ScreenToWorldPoint(Input.mousePosition);
        var touchPos = Input.mousePosition;
            
        if( Input.GetMouseButtonDown(0) )
        {
            OnTouchBegan(touchPos);
        }
        else if( Input.GetMouseButton(0) )
        {
            OnTouchMoved(touchPos);
        }
        else if( Input.GetMouseButtonUp(0) )
        {
            OnTouchEnded(touchPos);
        }
    }
}