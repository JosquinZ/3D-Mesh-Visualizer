using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class ModelController : MonoBehaviour
{
    [SerializeField]
    private List<ModelData> modelList = new List<ModelData>();

    [SerializeField]
    private Transform pivotTransform;

    [SerializeField]
    private Volume sceneVolume;

    [SerializeField]
    private float scaleUpSpeed;


    [SerializeField]
    private SideMenuController _sideMenuController;

    private GameObject currentModel;
    private ModelData currentModelData;

    void Start()
    {
        ClearModels();
        LoadModels();
    }


    private void LoadModels()
    {
        for (int i = 0; i < modelList.Count; i++)
        {
            GameObject newModel = GameObject.Instantiate(modelList[i].modelGameObject);
            newModel.transform.SetParent(pivotTransform);
            newModel.name = modelList[i].modelName;
            newModel.SetActive(false);

            int index = i;

            _sideMenuController.AddButtonToModelList(modelList[index]);
        }

        StartCoroutine(ResetModel(modelList[0]));
    }

    private void ClearModels()
    {
        foreach (Transform t in pivotTransform)
        {
            Destroy(t.gameObject);
        }
    }

    public void ResetCurrentModel()
    {
        if(currentModelData)
        {
            StopCoroutine("ResetModel");
            StartCoroutine(ResetModel(currentModelData));
        }
    }


    public IEnumerator ResetModel(ModelData modelData)
    {
        if(currentModel)
        {
            currentModel.SetActive(false);
        }

        pivotTransform.localScale = Vector3.one;
        pivotTransform.rotation = Quaternion.identity;

        Transform modelTransform = pivotTransform.Find(modelData.modelName);
        modelTransform.gameObject.SetActive(true);
        modelTransform.localScale = Vector3.zero;
        yield return new WaitForEndOfFrame();
        Debug.Log("modelTransform.localScale: " + modelTransform.localScale);


        //Post Processing profile attached to the model
        if (modelData.modelForcedProfile != null)
        {
            sceneVolume.profile = modelData.modelForcedProfile;
        }

        //Skybox Color
        if(modelData.forceSkyColor)
        {
            RenderSettings.skybox.DOBlendableColor(modelData.skyColorBottom, "_BottomColor", 2f);
            RenderSettings.skybox.DOBlendableColor(modelData.skyColorTop, "_TopColor", 2f);
        }



        currentModel = modelTransform.gameObject;
        currentModelData = modelData;

        BoxCollider boxCollider = modelTransform.GetComponent<BoxCollider>();

        bool inFrustrum = true;
        Camera mainCamera = Camera.main;

        while(inFrustrum)
        {
            yield return new WaitForEndOfFrame();
            modelTransform.localScale += Vector3.one * Time.deltaTime * scaleUpSpeed;

            if (mainCamera.WorldToViewportPoint(boxCollider.bounds.max).y > 1f
            ||  mainCamera.WorldToViewportPoint(boxCollider.bounds.max).x > 1f
            ||  mainCamera.WorldToViewportPoint(boxCollider.bounds.min).y < 0
            ||  mainCamera.WorldToViewportPoint(boxCollider.bounds.min).x < 0)
            {
                inFrustrum = false;

            }
        }
    }

    void Update()
    {
        
    }
}
