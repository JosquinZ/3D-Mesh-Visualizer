using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LightingController : MonoBehaviour
{
    [SerializeField]
    private Transform directionnalLightTransform;

    [SerializeField]
    private GameObject spotLights;

    void Start()
    {
        spotLights.SetActive(false);
    }

    public void SetDay()
    {
        spotLights.SetActive(false);
        RenderSettings.ambientLight = new Color(.9f,.9f,.9f);
        directionnalLightTransform.DORotate(new Vector3(45, 130, 90), 2f).SetEase(Ease.OutBack);
        directionnalLightTransform.GetComponent<Light>().DOIntensity(2, 2f);
    }


    public void SetNight()
    {
        spotLights.SetActive(false);
        RenderSettings.ambientLight = new Color(.43f, .45f, 1f);
        directionnalLightTransform.DORotate(new Vector3(270, 0, 90), 2f).SetEase(Ease.OutBack);
        directionnalLightTransform.GetComponent<Light>().DOIntensity(0, 2f);
    }


    public void SetSpotLight()
    {
        RenderSettings.ambientLight = Color.black;
        directionnalLightTransform.GetComponent<Light>().DOIntensity(0, .5f);
        spotLights.SetActive(true);
    }
}
