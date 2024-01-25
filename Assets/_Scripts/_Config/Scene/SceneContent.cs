using System;
using _Config.So;
using _Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class SceneContent : MonoBehaviour,ISceneContent
{
    [SerializeField] private Transform _bg;
    [SerializeField] private Animation _animation;
    public Transform Bg => _bg;
    public UnityEvent<Role.Index, int> OnRoleLineEvent { get; } = new UnityEvent<Role.Index, int>();
    public UnityEvent OnEndEvent { get; } = new UnityEvent();
    private void OnAnimEnd() => OnEndEvent.Invoke();
    private void OnLeftRoleLine(int lineIndex = 0) => OnRoleLineEvent.Invoke(Role.Index.Left, lineIndex);
    private void OnRightRoleLine(int lineIndex = 0) => OnRoleLineEvent.Invoke(Role.Index.Right, lineIndex);
    private void OnSoloRoleLine(int lineIndex = 0) => OnRoleLineEvent.Invoke(Role.Index.Solo, lineIndex);

    public void Play()
    {
        if (_animation.isPlaying)
            throw new NotImplementedException("Animation is playing!");
        _animation.DOPlay();
    }
}