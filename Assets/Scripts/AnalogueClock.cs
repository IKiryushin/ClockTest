using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnalogueClock : MonoBehaviour
{
    public Transform hourHand;
    public Transform minuteHand;
    public Transform secondHand;

    private DateTime currentTime;

    public bool handIsDraged = false;

    private void Start()
    {
        hourHand.GetComponent<UIDragRotate>()._analogueClock = this;
        minuteHand.GetComponent<UIDragRotate>()._analogueClock = this;
        DotUpdate();

    }

    void DotUpdate()
    {

        UpdateClockHands();

        DOVirtual.DelayedCall(1f, () =>
        {
            UpdateClockHands();
            DotUpdate();
        });
    }

    void UpdateClockHands(bool instant = false)
    {
        if (handIsDraged == true)
            return;

        currentTime = ClockManager.currentTime;

        float hours = currentTime.Hour % 12 + currentTime.Minute / 60f;
        float minutes = currentTime.Minute + currentTime.Second / 60f;
        float seconds = currentTime.Second + currentTime.Millisecond / 1000f;

        // Calculate the rotation angles
        float hourAngle = (hours * 30f) ; 
        float minuteAngle = (minutes * 6f) ; 
        float secondAngle = seconds * 6f;

        if (instant)
        {
            // Instantly set the rotation without animation
            hourHand.localRotation = Quaternion.Euler(0, 0, -hourAngle);
            minuteHand.localRotation = Quaternion.Euler(0, 0, -minuteAngle);
            secondHand.localRotation = Quaternion.Euler(0, 0, -secondAngle);
        }
        else
        {
            // Smoothly animate the rotation using DoTween
            hourHand.DOLocalRotate(new Vector3(0, 0, -hourAngle), 1f, RotateMode.Fast).SetEase(Ease.OutBack);
            minuteHand.DOLocalRotate(new Vector3(0, 0, -minuteAngle), 1f, RotateMode.Fast).SetEase(Ease.OutBack);
            secondHand.DOLocalRotate(new Vector3(0, 0, -secondAngle), 0.5f, RotateMode.Fast).SetEase(Ease.OutBack);
        }
    }

    public void OnDragEnd()
    {
        GetTimeFromClockHands();


        
        handIsDraged = false;
    }

    void GetTimeFromClockHands()
    {
        DateTime time = ClockManager.currentTime;

        float hourAngle = hourHand.rotation.eulerAngles.z;
        float minuteAngle = minuteHand.rotation.eulerAngles.z;

        Debug.Log($"hours A: {hourAngle} minutes A: {minuteAngle}");

        int hours = (int)-((hourAngle/30)-12); 
        int minutes = (int)-((minuteAngle / 6) - 60);


        Debug.Log($"hours: {hours} minutes: {minutes}");

        time = new DateTime(time.Year, time.Month, time.Day, hours, minutes, 0);

        ClockManager.currentTime = time;
    }

    private int ClockHandHelper(float angle)
    {
        return angle switch
        {
            float when (angle > -15 && angle < 15) => 0,
            float when (angle < -15 && angle > -45) => 1,
            float when (angle < -45 && angle > -75) => 2,
            float when (angle < -75 && angle > -105) => 3,
            float when (angle < -105 && angle > -135) => 4,
            float when (angle < -135 && angle > -165) => 5,
            float when (Math.Abs(angle) > 165) => 6,
            float when (angle > 135 && angle < 165) => 7,
            float when (angle > 105 && angle < 135) => 8,
            float when (angle > 75 && angle < 105) => 9,
            float when (angle > 45 && angle < 75) => 10,
            float when (angle > 15 && angle < 45) => 11,
            _ => 0,
        };
    }
}
