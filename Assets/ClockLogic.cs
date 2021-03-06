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
    private Transform m_secondArm;
    private Transform m_destinationSecondArm;
    [SerializeField]
    private bool m_secondTickMovement;
    [SerializeField]
    private AudioClip _m_secondTickSound;

    [SerializeField]
    private bool m_isRunning;
    [SerializeField]
    private bool m_startAtRealTime;
    [SerializeField]
    private float m_runningRealtimeMultiplier = 1f;
    public float RunningRealtimeMultipler { get => m_runningRealtimeMultiplier; set => SetRealtimeMultiplier(value); }
    private void SetRealtimeMultiplier(float value)
    {
        m_runningRealtimeMultiplier = value;
    }

    [SerializeField]
    private AudioSource m_clockAudio;

    [SerializeField] 
    private Coroutine m_rotateOverTimeCoroutine = null;

    private void Awake()
    {
        m_destinationHourArm = Instantiate(m_hourArm, m_hourArm.transform);
        m_destinationHourArm.GetComponentInChildren<Renderer>().enabled = false;

        m_destinationMinuteArm = Instantiate(m_minuteArm, m_minuteArm.transform);
        m_destinationMinuteArm.GetComponentInChildren<Renderer>().enabled = false;

        m_destinationSecondArm = Instantiate(m_secondArm, m_secondArm.transform);
        m_destinationSecondArm.GetComponentInChildren<Renderer>().enabled = false;
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

    public void SetTime(int hours, int minutes, int seconds = 0)
    {
        m_destinationHourArm.localEulerAngles = new Vector3(0f, 0f, -30f * (hours + (minutes / 60f) + (seconds / 3600f)));
        m_destinationMinuteArm.localEulerAngles = new Vector3(0f, 0f, -6f * (minutes + (seconds / 60f)));
        m_destinationSecondArm.localEulerAngles = new Vector3(0f, 0f, -6f * seconds);

        m_hourArm.localEulerAngles = m_destinationHourArm.localEulerAngles;
        m_minuteArm.localEulerAngles = m_destinationMinuteArm.localEulerAngles;
        m_secondArm.localEulerAngles = m_destinationSecondArm.localEulerAngles;
    }

    public void RotateToTime(float secondsToTake, int hours, int minutes, int seconds = 0)
    {
        if (secondsToTake < float.Epsilon)
        {
            SetTime(hours, minutes, seconds);
        }
        else
        {
            m_rotateOverTimeCoroutine = StartCoroutine(RotateOverTimeCoroutine(secondsToTake, hours, minutes, seconds));
        }
    }

    private IEnumerator RotateOverTimeCoroutine(float secondsToTake, int hours, int minutes, int seconds)
    {
        m_destinationHourArm.localEulerAngles = new Vector3(0f, 0f, -30f * hours);
        m_destinationMinuteArm.localEulerAngles = new Vector3(0f, 0f, -6f * minutes);
        m_destinationSecondArm.localEulerAngles = new Vector3(0f, 0f, -6f * seconds);

        float secondsElapsed = 0f;
        while (secondsElapsed < secondsToTake)
        {
            yield return null;
            float timeFraction = secondsElapsed / secondsToTake;
            m_hourArm.rotation = Quaternion.Lerp(m_hourArm.rotation, m_destinationHourArm.rotation, timeFraction);
            m_minuteArm.rotation = Quaternion.Lerp(m_minuteArm.rotation, m_destinationMinuteArm.rotation, timeFraction);
            m_secondArm.rotation = Quaternion.Lerp(m_secondArm.rotation, m_destinationSecondArm.rotation, timeFraction);
            secondsElapsed += Time.deltaTime;
        }

        m_destinationHourArm.localEulerAngles = m_hourArm.localEulerAngles;
        m_destinationMinuteArm.localEulerAngles = m_minuteArm.localEulerAngles;
        m_destinationSecondArm.localEulerAngles = m_secondArm.localEulerAngles;
        m_rotateOverTimeCoroutine = null;
    }

    private float? m_lastSecondRotationZ;
    private void Update()
    {
        if (m_runningRealtimeMultiplier > float.Epsilon)
        {
            if (m_isRunning && m_rotateOverTimeCoroutine == null)
            {
                m_destinationHourArm.Rotate(new Vector3(0f, 0f, ((-m_runningRealtimeMultiplier * 30f) / 3600f) * Time.deltaTime));
                m_destinationMinuteArm.Rotate(new Vector3(0f, 0f, ((-m_runningRealtimeMultiplier * 6f) / 60f) * Time.deltaTime));
                m_destinationSecondArm.Rotate(new Vector3(0f, 0f, (-m_runningRealtimeMultiplier * 6f) * Time.deltaTime));

                m_hourArm.localEulerAngles = m_destinationHourArm.localEulerAngles;
                m_minuteArm.localEulerAngles = m_destinationMinuteArm.localEulerAngles;
                if (m_secondTickMovement)
                {
                    m_lastSecondRotationZ = m_secondArm.localEulerAngles.z;

                    m_secondArm.localEulerAngles = new Vector3(
                        m_destinationSecondArm.localEulerAngles.x, 
                        m_destinationSecondArm.localEulerAngles.y, 
                        (int)(m_destinationSecondArm.localEulerAngles.z / 6f) * 6);

                    if (m_lastSecondRotationZ.HasValue && !Mathf.Approximately(m_secondArm.localEulerAngles.z, m_lastSecondRotationZ.Value))
                    {
                        m_clockAudio.PlayOneShot(_m_secondTickSound);
                    }
                }
                else
                {
                    m_secondArm.localEulerAngles = m_destinationSecondArm.localEulerAngles;
                }
            }
        }
    }
}
