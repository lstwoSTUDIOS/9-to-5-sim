using System;
using UnityEngine;

public class ObjectivesPanel : MonoBehaviour
{
    public float closedXPos;
    public float openedXPos;
    public float openSpeed;

    private RectTransform rectTransform;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            rectTransform.anchoredPosition = new(Mathf.Lerp(rectTransform.anchoredPosition.x, openedXPos, Time.deltaTime * openSpeed), 0f);
        }
        else
        {
            rectTransform.anchoredPosition = new(Mathf.Lerp(rectTransform.anchoredPosition.x, closedXPos, Time.deltaTime * openSpeed), 0f);
        }
    }
}
