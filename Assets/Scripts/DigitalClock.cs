using System;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class DigitalClock : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_InputField inputField;
    public TMP_Text errorMessageText; // Text to display error messages

    // Duration to show the error message
    public float errorMessageDuration = 2f;

    // Update is called once per frame
    void Update()
    {
        // Update the displayed time
        if (timeText != null)
        {
            timeText.text = ClockManager.currentTime.ToString("HH:mm:ss");
        }
    }

    public void OnEditTimeStart()
    {
        if (!ClockManager.overrideTime)
            return;

        inputField.gameObject.SetActive(true);
        inputField.text = timeText.text;
    }

    public void OnEditTimeEnd()
    {
        if (!ClockManager.overrideTime)
            return;

        inputField.gameObject.SetActive(false);

        DateTime time = ClockManager.currentTime;

        if (DateTime.TryParseExact(inputField.text, "HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime newTime))
        {
            time = new DateTime(time.Year, time.Month, time.Day, newTime.Hour, newTime.Minute, newTime.Second);
            ClockManager.currentTime = time;
        }
        else
        {
            ShowErrorMessage("Invalid time format. Please enter the time in HH:mm:ss format.");
        }
    }

    private void ShowErrorMessage(string message)
    {
        if (errorMessageText != null)
        {
            errorMessageText.text = message;
            errorMessageText.gameObject.SetActive(true);

            // Use DoTween to hide the error message after a few seconds
            errorMessageText.DOFade(0, errorMessageDuration).OnComplete(() =>
            {
                errorMessageText.gameObject.SetActive(false);
                errorMessageText.color = new Color(errorMessageText.color.r, errorMessageText.color.g, errorMessageText.color.b, 1); // Reset alpha
            });
        }
    }
}
