using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BackgroundController : MonoBehaviour
{
    [SerializeField]
    private BackgroundData defaultBackground;

    public void Start()
    {
        SetSkyboxColor(defaultBackground);
    }


    public void SetSkyboxColor(BackgroundData backgroundData)
    {
        RenderSettings.skybox.DOBlendableColor(backgroundData.bottomColor, "_BottomColor", 2f);
        RenderSettings.skybox.DOBlendableColor(backgroundData.topColor, "_TopColor", 2f);
    }

}
