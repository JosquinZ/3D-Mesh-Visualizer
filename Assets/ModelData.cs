using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu]
public class ModelData : ScriptableObject
{
    public string modelName;
    public GameObject modelGameObject;
    public VolumeProfile modelForcedProfile;

    public bool forceSkyColor = false;
    public Color skyColorTop;
    public Color skyColorBottom;
}
