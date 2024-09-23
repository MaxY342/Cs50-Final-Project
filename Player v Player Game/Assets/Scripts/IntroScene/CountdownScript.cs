using UnityEngine;
using System.Collections;
using TMPro; // Make sure TextMeshPro is included

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI countdownText; // Assign your TextMeshPro component here
    public float countdownTime = 10f; // Start countdown from 10 seconds

    private float currentTime;

    void Start()
    {
        currentTime = countdownTime; // Initialize current time with the start value
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        while (currentTime > 0)
        {
            countdownText.text = currentTime.ToString("0"); // Update the TextMeshPro text
            yield return new WaitForSeconds(1f); // Wait for 1 second
            currentTime--; // Decrease the time by 1 second
        }

        // When countdown reaches 0
        countdownText.text = "0";
        CountdownComplete(); // Optional: You can call a function when the countdown is done
    }

    void CountdownComplete()
    {
        Debug.Log("Countdown finished!");
        // Add additional logic for when the countdown ends
    }
}
