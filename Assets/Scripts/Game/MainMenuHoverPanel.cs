using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuHoverPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float openSpeed;
    public float targetY;
    public float startY;
    
    private RectTransform rectTransform;
    private bool isHovering;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (isHovering)
        {
            rectTransform.anchoredPosition = new(rectTransform.anchoredPosition.x, Mathf.Lerp(rectTransform.anchoredPosition.y, targetY, Time.deltaTime * openSpeed));
        }
        else
        {
            rectTransform.anchoredPosition = new(rectTransform.anchoredPosition.x, Mathf.Lerp(rectTransform.anchoredPosition.y, startY, Time.deltaTime * openSpeed));
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }
}
