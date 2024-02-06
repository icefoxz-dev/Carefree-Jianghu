using _Game;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IComicScene
{
    bool IsInScene(PointerEventData pointer);
}
//漫画场景。主要玩家交互与剧情演示的场景
public class ComicScene : MonoBehaviour,IComicScene
{
    [SerializeField] private Transform _bg;
    [SerializeField] private Camera _sceneCamera;
    public RectTransform Content;
    private RectTransform RectTransform { get; set; }
    private void Start()
    {
        RectTransform = (RectTransform)transform;
    }

    public bool IsInScene(PointerEventData pointer) => Game.IsInRect(RectTransform, pointer.position, _sceneCamera);
}