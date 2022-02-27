using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace DSPMarker
{

    public class UIClickHandler : MonoBehaviour, IPointerClickHandler
    {
        public UnityEvent onLeftClick2;
        public UnityEvent onRightClick2;
        public UnityEvent onMiddleClick2;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                MarkerList.OnRightClick(eventData.pointerPress);
            }
            else if (eventData.button == PointerEventData.InputButton.Middle)
            {
            }
        }
    }
}