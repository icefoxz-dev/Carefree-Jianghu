using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFrame : MonoBehaviour
{
    [SerializeField] private Transform _canvas;
    private SceneContent _content;
    public int FrameId { get; private set; } = -1;

    void Start()
    {
        FrameId = int.Parse(name.Split('_')[1]);
    }

    public void SetContent(SceneContent content)
    {
        _content = content;
        _content.transform.SetParent(_canvas);
        _content.transform.localPosition = Vector3.zero;
        _content.transform.localScale = Vector3.one;
    }
}