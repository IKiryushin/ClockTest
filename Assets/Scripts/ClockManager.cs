using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class ClockManager : MonoBehaviour
{

    public static DateTime currentTime;
    public static bool overrideTime = false;
    private float nextSyncTime;

    private const string timeUrl = "https://yandex.com/time/sync.json";
    private const float syncInterval = 3600f; // Sync every hour

    void Start()
    {
        StartCoroutine(SyncTime());
    }

    void Update()
    {
        // Update the current time
        currentTime += TimeSpan.FromSeconds(Time.deltaTime);


        if (overrideTime == true)
            return;

        // Check if it's time to sync again
        if (Time.time >= nextSyncTime)
        {
            StartCoroutine(SyncTime());
        }
    }

    public void OverrideTime(bool setOverrideTime)
    {
        overrideTime = setOverrideTime;

        if(overrideTime == false)
            StartCoroutine(SyncTime());
    }

    IEnumerator SyncTime()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(timeUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error fetching time: " + request.error);
            }
            else
            {
                try
                {
                    // Parse the response JSON to get the time
                    string jsonResult = request.downloadHandler.text;
                    TimeData timeData = JsonUtility.FromJson<TimeData>(jsonResult);

                    // Get the current time from the JSON (assumes UTC timestamp in milliseconds)
                    long unixTimeMilliseconds = timeData.time;
                    currentTime = DateTimeOffset.FromUnixTimeMilliseconds(unixTimeMilliseconds).DateTime;

                    // Set the next sync time
                    nextSyncTime = Time.time + syncInterval;
                }
                catch (Exception e)
                {
                    Debug.LogError("Error parsing time data: " + e.Message);
                }
            }
        }
    }

    [Serializable]
    public class TimeData
    {
        public long time;
    }
}
