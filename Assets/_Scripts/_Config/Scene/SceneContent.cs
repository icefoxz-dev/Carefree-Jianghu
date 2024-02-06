using System;
using System.Collections;
using _Data;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class SceneContent : MonoBehaviour, ISceneContent
{
    [SerializeField] private Transform _bg;
    [SerializeField] private Animation _animation;
    [SerializeField] private Transform _preview;
    [SerializeField] private Transform _leftRole;
    [SerializeField] private Transform _rightRole;
    [SerializeField] private Transform _soloRole;
    public Transform Bg => _bg;
    public UnityEvent<RolePlacing.Index, string> OnRoleLineEvent { get; } = new UnityEvent<RolePlacing.Index, string>();
    public UnityEvent OnEndEvent { get; } = new UnityEvent();
    private Animator _leftRoleAnimator;
    private Animator _rightRoleAnimator;
    private Animator _soloRoleAnimator;

    //{roleIndex, animName}
    private void OnRoleAnim(string roleIndex_animName)
    {
        var cmd = roleIndex_animName.Split(',');
        var index = int.Parse(cmd[0]);
        var animName = cmd[1];
    }
    //{roleIndex, lineIndex}
    private void OnRoleLine(string roleIndex_lineIndex)
    {
        var cmd = roleIndex_lineIndex.Split(',');
        var index = int.Parse(cmd[0]);
        var line = cmd[1];
        if (int.TryParse(line, out var lineIndex))
        {
            //todo : 实现根据lineIndex找到对应的台词
            line = $"未实现获取台词方法！Line = {cmd[1]}";
        }
        OnRoleLineEvent.Invoke((RolePlacing.Index)index, line);
    }
    private void OnRoleAnim(RolePlacing.Index place, string animName)
    {
        var animator = place switch
        {
            RolePlacing.Index.Left => _leftRoleAnimator,
            RolePlacing.Index.Right => _rightRoleAnimator,
            RolePlacing.Index.Solo => _soloRoleAnimator,
            _ => throw new ArgumentOutOfRangeException(nameof(place), place, null)
        };
        animator.Play(animName);
    }

    public void DisplayPreview(bool display) => _preview.gameObject.SetActive(display);
    /// <summary>
    /// 播放animation列表中的动画。根据index播放对应的动画
    /// </summary>
    /// <param name="index"></param>
    [Button]
    public void Play(int index)
    {
        if (_animation.isPlaying)
        {
            _animation.Stop();
            StopAllCoroutines();
            OnEndEvent?.Invoke();//interrupted end
        }
        DisplayPreview(false);
        var i = 0;
        foreach (AnimationState state in _animation)
        {
            if (index == i)
            {
                _animation.clip = state.clip;
                break;
            }
            i++;
        }

        _animation.Play();
        StartCoroutine(WaitUntilFinish());
        return;

        IEnumerator WaitUntilFinish()
        {
            yield return new WaitUntil(() => !_animation.isPlaying);
            OnEndEvent.Invoke();//normal end
        }
    }

    public void SetRole(RolePlacing.Index place, ICharacter character)
    {
        var parent = GetParent(place);
        if (parent == null) Debug.LogError($"{name}.SceneContent place = {place} is null!", this);
        foreach (Transform tran in parent) 
            tran.gameObject.SetActive(false);
        var prefab = Instantiate(character.Prefab, parent);
        var roleTransform = prefab.transform;
        roleTransform.localPosition = Vector3.zero;
        roleTransform.localRotation = Quaternion.identity;
    }

    private Transform GetParent(RolePlacing.Index place)
    {
        var parent = place switch
        {
            RolePlacing.Index.Left => _leftRole,
            RolePlacing.Index.Right => _rightRole,
            RolePlacing.Index.Solo => _soloRole,
            _ => throw new ArgumentOutOfRangeException(nameof(place), place, null)
        };
        return parent;
    }

    public void SetOrientation(RolePlacing.Index place, RolePlacing.Facing selection)
    {
        var parent = GetParent(place);
        var child = parent.GetChild(0);
        var scale = GetScale(place, selection);
        var localScale = child.localScale;
        child.localScale = new Vector3(scale * Mathf.Abs(localScale.x), localScale.y, localScale.z);
    }

    private float GetScale(RolePlacing.Index place, RolePlacing.Facing selection)
    {
        return place switch
        {
            RolePlacing.Index.Left => selection == RolePlacing.Facing.Front ? 1 : -1,
            RolePlacing.Index.Right => selection == RolePlacing.Facing.Front ? -1 : 1,
            RolePlacing.Index.Solo => 1,
            _ => throw new ArgumentOutOfRangeException(nameof(place), place, null)
        };
    }
}