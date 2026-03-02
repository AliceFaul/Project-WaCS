using UnityEngine;

namespace Project.Systems.Game
{
    public class PauseSystem : MonoBehaviour
    {
        public static bool IsPaused { get; private set; }

        public static void PauseGame(bool isPause)
        {
            IsPaused = isPause;
        }
    }
}
