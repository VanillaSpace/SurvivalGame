using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float time;
    public float fullDayLength;
    public float startTime = 0.4f;
    private float timeRate;
    public Vector3 noon;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    void Start ()
    {
        initializeDay();
    }

    void Update ()
    {
        incrementTime();

        lightRotation();

        lightIntensity();

        colorEvaluation();

        toggleSunMoon();
    }

    private void initializeDay()
    {
        timeRate = 1.0f / fullDayLength;
        time = startTime;
    }

    private void incrementTime()
    {
        // increment time
        time += timeRate * Time.deltaTime;

        if (time >= 1.0f)
            time = 0.0f;
    }

    private void lightRotation()
    {
        sun.transform.eulerAngles = (time - 0.25f) * noon * 4.0f;
        moon.transform.eulerAngles = (time - 0.75f) * noon * 4.0f;
    }

    private void lightIntensity()
    {
        // Don't need for sun
        //sun.intensity = sunIntensity.Evaluate(time);
        moon.intensity = moonIntensity.Evaluate(time);
    }

    private void colorEvaluation()
    {
        sun.color = sunColor.Evaluate(time);
        moon.color = moonColor.Evaluate(time);
    }

    private void toggleSunMoon()
    {
        // enable / disable the sun
        if (sun.intensity == 0 && sun.gameObject.activeInHierarchy)
        {
            sun.gameObject.SetActive(false);
        }
        else if (sun.intensity > 0 && !sun.gameObject.activeInHierarchy)
        {
            sun.gameObject.SetActive(true);
        }
    }
}
