using UnityEngine;
using UnityEngine.SceneManagement;

namespace AlarmClock.Scripts
{
    public class AppBootstrap : MonoBehaviour
    {
        private void Start()
        {
            AppData.ClockTimeProvider.OnInitialized += LoadNextScene;
        }

        private void OnDestroy()
        {
            AppData.ClockTimeProvider.OnInitialized -= LoadNextScene;
        }

        private void LoadNextScene() 
            => LoadSceneAsync(1);

        private static void LoadSceneAsync(int sceneIndex) 
            => SceneManager.LoadSceneAsync(sceneIndex);
    }
}