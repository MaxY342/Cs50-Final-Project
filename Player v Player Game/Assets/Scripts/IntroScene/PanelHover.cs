using UnityEngine;
using UnityEngine.EventSystems;

public class PanelHover : MonoBehaviour
{
    private HoverManager HoverManager;

    void Start()
    {
        HoverManager = FindObjectOfType<HoverManager>();
    }

    public void OnPointerEnter()
    {
        HoverManager.ShowButtonsPanel();
    }
}
