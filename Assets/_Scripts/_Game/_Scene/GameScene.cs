using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public interface IGameScene
{
    SceneFrame GetFrame(int index);
}

/// <summary>
/// 游戏场景
/// </summary>
public class GameScene : MonoBehaviour, IGameScene
{
    /*
     * 1.管理摄像头- a.场景摄像头(全局摄像头)，b.重点(中间)摄像头
     */
    [SerializeField] private Camera _sceneCamera;//场景摄像头
    [SerializeField] private Camera _focusCamera;//重点摄像头
    [SerializeField] private FrameField _frame;
    [SerializeField] private int _frameDiv = 5;//场景单位分割

    public void Init()
    {
        StartCoroutine(LateInit());
        return;

        IEnumerator LateInit()
        {
            yield return new WaitForSeconds(0.2f);
            FramesAlignment();
        }
    }

    [Button,GUIColor("red")] public void FramesAlignment()
    {
        foreach (var frame in _frame.List) 
            AlignByParentSize(frame.Content, frame.RectTransform, _frameDiv);
    }

    private static void AlignByParentSize(Transform tran, RectTransform parent, float div)
    {
        var parentRect = parent.rect;
        tran.localScale = new Vector3(parentRect.width / div, parentRect.height / div, 1);
    }

    public SceneFrame GetFrame(int index) => _frame.List[index];

    [Serializable]
    private class FrameField
    {
        [SerializeField] private SceneFrame _frame1;
        [SerializeField] private SceneFrame _frame2;
        [SerializeField] private SceneFrame _frame3;
        [SerializeField] private SceneFrame _frame4;
        [SerializeField] private SceneFrame _frame5;
        [SerializeField] private SceneFrame _frame6;
        public SceneFrame SceneFrame1 => _frame1;
        public SceneFrame SceneFrame2 => _frame2;
        public SceneFrame SceneFrame3 => _frame3;
        public SceneFrame SceneFrame4 => _frame4;
        public SceneFrame SceneFrame5 => _frame5;
        public SceneFrame SceneFrame6 => _frame6;
        public SceneFrame[] List => new[] { _frame1, _frame2, _frame3, _frame4, _frame5, _frame6 };
    }
}