using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player_v_Player_Game.IntroScene
{
    public class StartButton : MonoBehaviour
    {
        public void OnStartButtonClick()
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
