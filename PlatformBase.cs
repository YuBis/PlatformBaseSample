using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using SingletonPattern;

public class PlatformBase<T> : Singleton<T> where T : MonoBehaviour
{
    private readonly float m_kDoubleClickAllowDistance = 10;
    private readonly float m_kDoubleClickAllowTime = 0.3f;
    private float m_doubleClickCheckTime;
    private ITouchable m_pickingObject;
    private Vector3 m_beforeTouchedPos = new Vector3();
    private Vector3 m_touchBeganPos = new Vector3();
    private Vector3 m_posForCameraMoving = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        RenewLastClickedTime(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected bool CheckDoubleTap(in Vector3 kTouchedPos)
    {
        if( !CameraController.GetInstance.CanMapZoom() )
            return false;

        if( m_kDoubleClickAllowTime > Time.time - m_doubleClickCheckTime &&
            m_kDoubleClickAllowDistance > (kTouchedPos - m_beforeTouchedPos).magnitude )
        {
            RenewLastClickedTime(0);

            return true;
        }
        
        RenewLastClickedTime();

        return false;
    }

    protected void ScreenDrag(in Vector3 kNewPos)
    {
        var pickObj = Utility.GetTouchedObject(kNewPos, (param) => {
            if ((param as Tile)?.m_placeType == PlaceType.Floor)
                return true;

            return false;
        });
        switch(ModeManager.GetInstance.m_runningMode)
        {
            case Mode.EditTile : 
            {
                pickObj?.OnTouched();
            } break;
        }
    }

    protected void RenewLastClickedTime(float newTime = -1)
    {
        if( newTime == -1 )
            m_doubleClickCheckTime = Time.time;
        else
            m_doubleClickCheckTime = newTime;
    }

    protected void RenewLastClickedPos(in Vector3 kNewPos)
    {
        m_beforeTouchedPos = kNewPos;
    }

    protected void OnTouchBegan(in Vector3 kTouchedPos)
    {
        m_touchBeganPos = m_posForCameraMoving = kTouchedPos;
        m_pickingObject = Utility.GetTouchedObject(kTouchedPos);
    }

    protected void OnTouchMoved(in Vector3 kTouchedPos)
    {
        if( CameraController.GetInstance.CanCamMove() )
        {
            if( m_posForCameraMoving != kTouchedPos )
            {
                CameraController.GetInstance.MoveCameraBy((m_posForCameraMoving - kTouchedPos));
                m_posForCameraMoving = kTouchedPos;
                //Debug.Log($"cammovpos:{m_posForCameraMoving}, touchpos:{kTouchedPos}");
            }
        }
        else
        {
            ScreenDrag(kTouchedPos);
        }
    }

    protected void OnTouchEnded(in Vector3 kTouchedPos)
    {
        if( CheckDoubleTap(kTouchedPos) )
        {
            CameraController.GetInstance.ZoomCamera(kTouchedPos);
            return;
        }

        if( m_pickingObject == Utility.GetTouchedObject(kTouchedPos) &&
            m_kDoubleClickAllowDistance > (kTouchedPos - m_touchBeganPos).magnitude )
        {
            m_pickingObject?.OnTouched();
        }

        ResetClickInfo(kTouchedPos);
    }

    private void ResetClickInfo(in Vector3 kTouchedPos)
    {
        RenewLastClickedPos(kTouchedPos);
        RenewLastClickedTime();
        m_pickingObject = null;
    }

    public virtual void CheckOnTouches(){ throw new NotImplementedException(); }
}