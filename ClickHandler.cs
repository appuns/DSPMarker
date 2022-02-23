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
                //LogManager.Logger.LogInfo("---------------------------------------------------------onLeftClick");

                //onLeftClick2.Invoke();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                //LogManager.Logger.LogInfo("---------------------------------------------------------onRightClick");
                MarkerList.OnRightClick(eventData.pointerPress);
            }
            else if (eventData.button == PointerEventData.InputButton.Middle)
            {
                //LogManager.Logger.LogInfo("---------------------------------------------------------onMiddleClick" );
                //onMiddleClick2.Invoke();
            }
            //if (eventData.pointerPress != null)
            //{
            //    LogManager.Logger.LogInfo("---------------------------------------------------------" + eventData.pointerPress.name);
            //}
        }
    }
}