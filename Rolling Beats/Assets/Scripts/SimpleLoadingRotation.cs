using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLoadingRotation : MonoBehaviour
{
    [SerializeField] private RectTransform mainIcon;
    private float timeStep = 0.05f;
    private float oneStepAngle = -10;

    private float startTime;
    static bool loading;

    public static void FinishLoading()
    {
        loading = false;
    }

    void Start()
    {
        startTime = Time.time;
        mainIcon = GetComponent<RectTransform>();
        loading = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(loading)
            RotateSprite();
    }

    private void RotateSprite()
    {
        if(Time.time - startTime >= timeStep)
        {
            Vector3 iconAngle = mainIcon.localEulerAngles;
            iconAngle.z += oneStepAngle;
            
            mainIcon.localEulerAngles = iconAngle;

            startTime = Time.time;
        }
    }
}
