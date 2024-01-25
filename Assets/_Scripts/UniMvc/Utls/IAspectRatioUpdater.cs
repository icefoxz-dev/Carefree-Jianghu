using UnityEngine;

/// <summary>
/// 根据不同分辨率调整布局的接口
/// </summary>
public abstract class IAspectRatioUpdater : MonoBehaviour
{
    public abstract void UpdateAspectRatio();
}