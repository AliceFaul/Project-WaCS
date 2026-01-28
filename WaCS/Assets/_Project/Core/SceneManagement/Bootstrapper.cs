using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Core.SceneManagement
{
    public class Bootstrapper : MonoBehaviour
    {
        private static async void Init()
        {
            Debug.Log("Bootstrapper initialized");
            await SceneManager.LoadSceneAsync("Bootstrapper", LoadSceneMode.Single);
        }
    }
}