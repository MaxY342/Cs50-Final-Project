using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundHover : MonoBehaviour
{
    private HoverManager HoverManager;

    void Start()
    {
        HoverManager = FindObjectOfType<HoverManager>();
    }

    public void OnPointerEnter()
    {
        HoverManager.HideButtons();
    }
}
