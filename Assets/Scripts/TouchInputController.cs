using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputController : MonoBehaviour
{

    private enum TouchState
    {
        None,
        Rotate,
        Zoom,
        Translate
    }

    [SerializeField]
    private TouchState currentTouchState = TouchState.None;

    [SerializeField]
    private Transform pivotTransform;

    //Rotation
    [SerializeField]
    private float rotationSpeed = 1f;

    private Vector2 prevTouchPos;
    private Vector3 directionVect;
    private Vector3 rotationDirection;

    public Camera cam;
    [Space(10)]
    public float rotationMultiplier = 360.0f;
    public float slerpSpeed = 1.0f;
    public float releaseSlerpSpeed = 3.0f;
    private bool isRotating = false;


    //Zoom
    [SerializeField]
    private float zoomDeadZone = 1f;
    private bool isScaling = false;

    private float initialTouchesDistance;
    private float prevTouchesDistance;
    private Vector3 pivotInitialScale = Vector3.one;
    private Vector3 rotationRemaining;
    private float scaleFactor;
    private float scaleVelocity = 0;
    [SerializeField]
    private float scaleMultiplier;


    //Translate
    private Vector2 prevMidPoint;
    private bool isTranslating = false;
    [SerializeField]
    private Vector3 translateDirection;
    [SerializeField]
    private float translateMultiplier;
    private Vector3 translateVelocity = Vector3.zero;


    void Start()
    {
        
    }

    void Update()
    {
        if(Input.touchCount > 0)
        {
            if(Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                currentTouchState = TouchState.Rotate;

                if(isScaling)
                {
                    prevTouchPos = touch.position;
                    isScaling = false;
                    isTranslating = false;

                }

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        prevTouchPos = touch.position;
                        isRotating = true;

                        break;

                    case TouchPhase.Moved:
                        rotationDirection = touch.position - prevTouchPos;
                        rotationDirection.z = 0.0f;
                        prevTouchPos = touch.position;
                        break;
                    case TouchPhase.Stationary:
                        rotationDirection = Vector3.zero;
                        break;
                    case TouchPhase.Ended:
                        rotationDirection = Vector3.zero;
                        isRotating = false;

                        currentTouchState = TouchState.None;
                        break;
                    default:
                        break;
                }
            }
            else if(Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                if (touchOne.phase == TouchPhase.Began)
                {
                    isRotating = false;
                    initialTouchesDistance = Vector2.Distance(touchZero.position, touchOne.position);
                    prevTouchesDistance = Vector2.Distance(touchZero.position, touchOne.position);

                    pivotInitialScale = pivotTransform.localScale;
                    prevMidPoint = (touchZero.position + touchOne.position) / 2;
                }
                else
                {
                    float currentTouchesDistance = Vector2.Distance(touchZero.position, touchOne.position);
                    if(Mathf.Abs(prevTouchesDistance - currentTouchesDistance) < zoomDeadZone)
                    {
                        if(currentTouchState == TouchState.Zoom)
                        {
                            prevMidPoint = (touchZero.position + touchOne.position) / 2;
                            isScaling = false;
                        }
                        isTranslating = true;
                        currentTouchState = TouchState.Translate;
                        Vector2 midPoint = (touchZero.position + touchOne.position) / 2;


                        Vector3 delta = midPoint - prevMidPoint;
                        //pivotTransform.position += delta * 0.01f;
                        translateDirection = delta;
                        //pivotTransform.position = Vector3.Lerp(pivotTransform.position, pivotTransform.position += delta, Time.deltaTime * .1f);
                        prevMidPoint = midPoint;
                    }
                    else
                    {
                        if(currentTouchState == TouchState.Translate)
                        {
                            isTranslating = false;
                        }

                        currentTouchState = TouchState.Zoom;
                        isScaling = true;
                        //Set scale based on distance
                        //scaleDirection = pivotInitialScale * (currentTouchesDistance / initialTouchesDistance);
                        scaleFactor = currentTouchesDistance / initialTouchesDistance;
                        //pivotTransform.localScale = pivotInitialScale * (currentTouchesDistance / initialTouchesDistance);

                    }
                    prevTouchesDistance = Vector2.Distance(touchZero.position, touchOne.position);

                }

                if (touchZero.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Ended)
                {
                    initialTouchesDistance = Vector2.Distance(touchZero.position, touchOne.position);
                    pivotInitialScale = pivotTransform.localScale;
                }
            }
        }
        else
        {
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
            //scaleFactor = Vector3.SmoothDamp(scaleFactor, Vector3.zero, ref scaleVelocity, .25f);
            scaleFactor = Mathf.SmoothDamp(scaleFactor, 1, ref scaleVelocity, .25f);
        }

        pivotTransform.localScale = Vector3.Lerp(pivotTransform.localScale, pivotTransform.localScale * scaleFactor, Time.deltaTime * scaleMultiplier);

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
