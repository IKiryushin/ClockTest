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

    private void Start()
    {

            CallMethodRepeatedly();

    }

    void CallMethodRepeatedly()
    {

        UpdateClockHands();

        DOVirtual.DelayedCall(1f, () =>
        {
            UpdateClockHands();
            CallMethodRepeatedly();
        });
    }

    void UpdateClockHands(bool instant = false)
    {
        if (ClockManager.overrideTime == true)
            return;

        currentTime = ClockManager.currentTime;

        float hours = currentTime.Hour % 12 + currentTime.Minute / 60f;
        float minutes = currentTime.Minute + currentTime.Second / 60f;
        float seconds = currentTime.Second + currentTime.Millisecond / 1000f;

        // Calculate the rotation angles
        float hourAngle = (hours * 30f) -90f; // -90 = start position offset 
        float minuteAngle = (minutes * 6f) -90f; // -90 = start position offset 
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
            hourHand.DOLocalRotate(new Vector3(0, 0, -hourAngle), 1f, RotateMode.Fast);
            minuteHand.DOLocalRotate(new Vector3(0, 0, -minuteAngle), 1f, RotateMode.Fast);
            secondHand.DOLocalRotate(new Vector3(0, 0, -secondAngle), 0.5f, RotateMode.Fast);
        }
    }
}
