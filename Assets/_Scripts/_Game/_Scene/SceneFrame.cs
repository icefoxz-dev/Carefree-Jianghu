using System.Collections;
using System.Collections.Generic;
using _Data;
using UnityEngine;

public class SceneFrame : MonoBehaviour
{
    #region Scene & Cam ratio aligment
    public RectTransform RectTransform;
    public Transform Content;
    #endregion
    public SceneContent CurrentScene { get; private set; }

    public void SetSceneContent(ISceneContent sceneContent)
    {
        ClearSceneContent();
        var sc = Instantiate(sceneContent.gameObject, Content);
        sc.transform.localPosition = Vector3.zero;
        CurrentScene = sc.GetComponent<SceneContent>();
    }

    public void ClearSceneContent()
    {
        if (CurrentScene)
            Destroy(CurrentScene.gameObject);
    }
}