using UnityEngine;

public class HoverManager : MonoBehaviour
{
    public GameObject additionalButton1;
    public GameObject additionalButton2;

    private bool hashovered = false;

    void Start()
    {
        additionalButton1.SetActive(false);
        additionalButton2.SetActive(false);
    }

    public void ShowButtons()
    {
        additionalButton1.SetActive(true);
        additionalButton2.SetActive(true);
    }

    public void HideButtons()
    {
        additionalButton1.SetActive(false);
        additionalButton2.SetActive(false);
        hashovered = false;
    }
    public void ShowButtonsHover()
    {
        hashovered = true;
        ShowButtons();
    }
    public void ShowButtonsPanel()
    {
        if (hashovered)
        {
            ShowButtons();
        }
    }
}
