using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasToObject : MonoBehaviour
{
    private RectTransform rectTransform;


    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        var map = gameObject.transform.parent.transform.parent;
        rectTransform.anchoredPosition = map.localPosition;
    }
}
