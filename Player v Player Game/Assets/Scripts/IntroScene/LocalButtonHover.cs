using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour
{
    private HoverManager hoverManager;

    void Start()
    {
        hoverManager = FindObjectOfType<HoverManager>();
    }

    public void OnPointerEnter()
    {
        hoverManager.ShowButtonsHover();
    }
}