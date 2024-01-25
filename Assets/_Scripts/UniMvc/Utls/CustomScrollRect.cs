using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UniMvc.Utls
{
    public class CustomScrollRect : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private ScrollRect _parentScrollRect;
        private ScrollRect _ownScrollRect;
        private bool dragHorizontal = false;

        public void Init(ScrollRect parentScrollRect)
        {
            _ownScrollRect = GetComponent<ScrollRect>();
            if(!_ownScrollRect)
                throw new System.Exception("CustomScrollRect Init failed, no ScrollRect component found");
            _parentScrollRect = parentScrollRect;
        }

        private ScrollRect GetControlledScrollRect(bool isHorizontal)=> isHorizontal ? _ownScrollRect : _parentScrollRect;
        public void OnBeginDrag(PointerEventData eventData)
        {
            dragHorizontal = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y);
            GetControlledScrollRect(dragHorizontal).OnBeginDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            GetControlledScrollRect(dragHorizontal).OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            GetControlledScrollRect(dragHorizontal).OnEndDrag(eventData);
        }
    }
}

