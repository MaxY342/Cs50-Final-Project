using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverHandler : MonoBehaviour
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
