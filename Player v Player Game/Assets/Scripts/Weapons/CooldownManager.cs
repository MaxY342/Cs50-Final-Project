using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Player_v_Player_Game.Weapons
{
    public class CooldownManager
    {
        public IEnumerator ApplyCooldown(float duration, Image cooldownOverlay, TMP_Text cooldownText = null)
        {
            if (cooldownText != null)
            {
                cooldownText.gameObject.SetActive(true);
            }
            
            float remainingTime = duration;

            while (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;

                if (cooldownText != null)
                {
                    cooldownText.text = Mathf.RoundToInt(remainingTime).ToString();
                }

                cooldownOverlay.fillAmount = remainingTime / duration;
                yield return null;
            }

            if (cooldownText != null)
            {
                cooldownText.gameObject.SetActive(false);
            }
            
            cooldownOverlay.fillAmount = 0.0f;
        }
    }
}