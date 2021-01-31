using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using UnityEngine;

public class ClockLogic : MonoBehaviour
{
    [SerializeField]
    private Transform m_hourArm;
    private Transform m_destinationHourArm;

    [SerializeField]
    private Transform m_minuteArm;
    private Transform m_destinationMinuteArm;

    [SerializeField]
    private bool m_isRunning;
    [SerializeField]
    private bool m_startAtRealTime;
    [SerializeField]
    private float m_runningRealtimeMultiplier = 1f;

    [SerializeField] 
    private Coroutine m_rotateOverTimeCoroutine = null;

    private void Awake()
    {
        m_destinationHourArm = Instantiate(m_hourArm, m_hourArm.transform);
        m_destinationHourArm.GetComponentInChildren<Renderer>().enabled = false;

        m_destinationMinuteArm = Instantiate(m_minuteArm, m_minuteArm.transform);
        m_destinationMinuteArm.GetComponentInChildren<Renderer>().enabled = false;
    }

    private void Start()
    {
        if (m_startAtRealTime)
        {
            SetTime(DateTime.Now.Hour, DateTime.Now.Minute);
        }
    }

    public DateTime GetCurrentTime()
    {
        return new DateTime();
    }

    public void SetTime(int hours, int minutes)
    {
        m_hourArm.localEulerAngles = new Vector3(0f, 0f, -30f * (hours + (minutes / 60f)));

        m_minuteArm.localEulerAngles = new Vector3(0f, 0f, -6f * minutes);
    }

    public void RotateToTime(int hours, int minutes, float secondsToTake)
    {
        if (secondsToTake < float.Epsilon)
        {
            SetTime(hours, minutes);
        }
        else
        {
            m_rotateOverTimeCoroutine = StartCoroutine(RotateOverTimeCoroutine(hours, minutes, secondsToTake));
        }
    }

    private IEnumerator RotateOverTimeCoroutine(int hours, int minutes, float secondsToTake)
    {
        m_destinationHourArm.localEulerAngles = new Vector3(0f, 0f, -30f * hours);
        m_destinationMinuteArm.localEulerAngles = new Vector3(0f, 0f, -6f * minutes);

        float secondsElapsed = 0f;
        while (secondsElapsed < secondsToTake)
        {
            yield return null;
            float timeFraction = secondsElapsed / secondsToTake;
            m_hourArm.rotation = Quaternion.Lerp(m_hourArm.rotation, m_destinationHourArm.rotation, timeFraction);
            secondsElapsed += Time.deltaTime;
        }

        m_destinationHourArm.localEulerAngles = Vector3.zero;
        m_destinationMinuteArm.localEulerAngles = Vector3.zero;
        m_rotateOverTimeCoroutine = null;
    }

    private void Update()
    {
        if (m_runningRealtimeMultiplier > float.Epsilon)
        {
            if (m_isRunning && m_rotateOverTimeCoroutine == null)
            {
                m_minuteArm.Rotate(new Vector3(0f, 0f, (-m_runningRealtimeMultiplier / 60f) * Time.deltaTime));
                m_hourArm.Rotate(new Vector3(0f, 0f, ((-m_runningRealtimeMultiplier / 12f) / 60f) * Time.deltaTime));
            }
        }
    }
}
