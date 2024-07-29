using UnityEngine;
using UnityEngine.EventSystems;

public class PanelHoverHandler : MonoBehaviour
{
    private HoverManager HoverManager;

    void Start()
    {
        HoverManager = FindObjectOfType<HoverManager>();
    }

    public void OnPointerEnter()
    {
        HoverManager.ShowButtons();
    }

    public void OnPointerExit()
    {
        HoverManager.HideButtons();
    }
}
