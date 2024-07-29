using UnityEngine;

public class HoverManager : MonoBehaviour
{
    public GameObject additionalButton1;
    public GameObject additionalButton2;

    private int hoverCount = 0;

    void Start()
    {
        // Hide the additional buttons initially
        additionalButton1.SetActive(false);
        additionalButton2.SetActive(false);
    }

    public void ShowButtons()
    {
        hoverCount++;
        if (hoverCount == 1)
        {
            additionalButton1.SetActive(true);
            additionalButton2.SetActive(true);
        }
    }

    public void HideButtons()
    {
        hoverCount--;
        if (hoverCount == 0)
        {
            additionalButton1.SetActive(false);
            additionalButton2.SetActive(false);
        }
    }
}
