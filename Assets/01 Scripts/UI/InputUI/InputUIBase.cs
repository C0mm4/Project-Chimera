using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputUIBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private bool isClick;
    [SerializeField] private VirtualDPad dpad;
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isClick)
        {
            dpad.OnDragHandler(eventData);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect,
            eventData.position,
            eventData.pressEventCamera,
            out var localPoint
        );

        dpad.gameObject.SetActive(true);
        dpad.transform.localPosition = localPoint;
        isClick = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dpad.gameObject.SetActive(false);
        isClick = false;
    }
}
