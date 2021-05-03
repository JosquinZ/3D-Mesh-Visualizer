using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputController : MonoBehaviour
{
    [SerializeField]
    private Transform pivotTransform;

    [Header("Rotation")]
    private Vector2 prevTouchPos;
    private Vector3 directionVect;
    private Vector3 rotationDirection;

    [SerializeField]
    private float rotationMultiplier = 360.0f;

    [SerializeField]
    private float slerpSpeed = 1.0f;

    [SerializeField]
    private float releaseSlerpSpeed = 3.0f;
    private bool isRotating = false;


    [Space(10)]
    [Header("Scale")]

    [SerializeField]
    private float zoomDeadZone = 1f;
    private bool isScaling = false;
    private float initialTouchesDistance;
    private float prevTouchesDistance;
    private Vector3 pivotInitialScale = Vector3.one;
    private float scaleFactor;
    private float scaleVelocity = 0;
    [SerializeField]
    private float scaleMultiplier;

    [Space(10)]
    [Header("Translate")]
    private Vector2 prevMidPoint;
    private bool isTranslating = false;
    [SerializeField]
    private Vector3 translateDirection;
    [SerializeField]
    private float translateMultiplier;
    private Vector3 translateVelocity = Vector3.zero;

    void Update()
    {
        if(Input.touchCount > 0)
        {
            if(Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);

                if(isScaling)
                {
                    isScaling = false;
                }
                if(isTranslating)
                {
                    isTranslating = false;
                }

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        isRotating = true;
                        break;
                    case TouchPhase.Moved:
                        rotationDirection = touch.deltaPosition;
                        rotationDirection.z = 0.0f;
                        break;
                    case TouchPhase.Stationary:
                        rotationDirection = Vector3.zero;
                        isRotating = false;
                        break;
                    case TouchPhase.Ended:
                        rotationDirection = Vector3.zero;
                        isRotating = false;
                        break;
                }
            }
            else if(Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                if (touchOne.phase == TouchPhase.Began)
                {
                    rotationDirection = Vector3.zero;
                    isRotating = false;
                    initialTouchesDistance = Vector2.Distance(touchZero.position, touchOne.position);
                    prevTouchesDistance = Vector2.Distance(touchZero.position, touchOne.position);

                    pivotInitialScale = pivotTransform.localScale;
                    prevMidPoint = (touchZero.position + touchOne.position) / 2;
                }
                else if(touchZero.phase != TouchPhase.Moved || touchOne.phase == TouchPhase.Moved
                    || touchZero.phase == TouchPhase.Stationary || touchOne.phase == TouchPhase.Stationary)
                {
                    isScaling = true;
                    isTranslating = true;

                    float currentTouchesDistance = Vector2.Distance(touchZero.position, touchOne.position);

                    Vector2 midPoint = (touchZero.position + touchOne.position) / 2;

                    if (Mathf.Abs(prevTouchesDistance - currentTouchesDistance) < zoomDeadZone)
                    {
                        Vector3 delta = midPoint - prevMidPoint;
                        translateDirection = delta;
                        prevMidPoint = midPoint;
                    }

                    scaleFactor = currentTouchesDistance / initialTouchesDistance;
                    pivotTransform.localScale = pivotInitialScale * scaleFactor;
                    prevMidPoint = (touchZero.position + touchOne.position) / 2;
                    prevTouchesDistance = Vector2.Distance(touchZero.position, touchOne.position);
                }
            }
        }
        else
        {
            isRotating = false;
            isTranslating = false;
            isScaling = false;
        }

        UpdateRotation();
        UpdateScaling();
        UpdateTranslate();
    }

    void UpdateRotation()
    {
        float wantedSlerpSpeed = isRotating ? slerpSpeed : releaseSlerpSpeed;
        directionVect = Vector3.Lerp(directionVect, rotationDirection, Time.deltaTime * wantedSlerpSpeed);

        if (Vector3.Dot(pivotTransform.up, Vector3.up) >= 0)
        {
            pivotTransform.Rotate(new Vector3(0, -directionVect.x * rotationMultiplier, 0.0f), Space.Self);
        }
        else
        {
            pivotTransform.Rotate(new Vector3(0, directionVect.x * rotationMultiplier, 0.0f), Space.Self);
        }
        pivotTransform.Rotate(new Vector3(directionVect.y * rotationMultiplier, 0, 0.0f), Space.World);
    }

    void UpdateScaling()
    {
        if(!isScaling)
        {
            scaleFactor = Mathf.SmoothDamp(scaleFactor, 1f, ref scaleVelocity, .25f);
            pivotTransform.localScale = Vector3.Lerp(pivotTransform.localScale, pivotTransform.localScale * scaleFactor, Time.deltaTime * scaleMultiplier);
        }
    }

    void UpdateTranslate()
    {
        if(!isTranslating)
        {
            translateDirection = Vector3.SmoothDamp(translateDirection, Vector3.zero, ref translateVelocity, .25f);
        }
        pivotTransform.position = Vector3.Lerp(pivotTransform.position, pivotTransform.position + translateDirection, Time.deltaTime * translateMultiplier);
    }
}
