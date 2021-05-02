using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SideMenuController : MonoBehaviour
{
    [SerializeField]
    private float animationDuration;
    private float currentAnimationTime;
    private Vector3 initialPosition;
    private RectTransform sideMenuRect;

    [SerializeField]
    private Transform modelListTransform;

    [SerializeField]
    private GameObject modelButtonPrefab;

    [SerializeField]
    private ModelController modelController;

    private bool menuOpen = false;

    void Start()
    {
        sideMenuRect = GetComponent<RectTransform>();
        sideMenuRect.anchoredPosition = new Vector2(-Screen.width / 2, 0);
    }

    [ContextMenu("ToggleMenu")]
    public void ToggleMenu()
    {
        if(menuOpen)
        {
            StartCoroutine(Animate(new Vector2(-Screen.width / 2, 0)));
        }
        else
        {
            StartCoroutine(Animate(new Vector2(0, 0)));
        }
    }


    IEnumerator Animate(Vector3 targetPosition)
    {
        float time = 0;
        Vector2 rectStartPosition = sideMenuRect.anchoredPosition;

        while(time < animationDuration)
        {
            sideMenuRect.anchoredPosition = Vector2.Lerp(rectStartPosition, targetPosition, time / animationDuration);
            time += Time.deltaTime;
            yield return null;
        }

        sideMenuRect.anchoredPosition = targetPosition;
        menuOpen = !menuOpen;
    }

    public void AddButtonToModelList(ModelData modelData)
    {
        GameObject newButton = GameObject.Instantiate(modelButtonPrefab, modelListTransform);
        newButton.GetComponentInChildren<TMP_Text>().text = modelData.modelName;

        modelController.StopAllCoroutines();
        newButton.GetComponent<Button>().onClick.AddListener(() => { StartCoroutine(modelController.ResetModel(modelData)); });
    }
}
