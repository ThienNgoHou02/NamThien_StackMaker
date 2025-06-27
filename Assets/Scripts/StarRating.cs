using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarRating : MonoBehaviour
{
    [SerializeField] private float scalingSpeed;
    [SerializeField] private Vector2 targetSize;

    private RectTransform rectTransform;
    private bool isScaling;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = Vector2.zero;
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        isScaling = true;
    }
    private void Update()
    {
        if (isScaling)
        {
            rectTransform.sizeDelta = Vector2.Lerp(rectTransform.sizeDelta, targetSize, scalingSpeed * Time.deltaTime);
            Vector2 scale = new Vector2(
                    Mathf.Clamp(rectTransform.sizeDelta.x, 0f, targetSize.x),
                    Mathf.Clamp(rectTransform.sizeDelta.y, 0f, targetSize.y));

            rectTransform.sizeDelta = scale;
        }
    }
}
