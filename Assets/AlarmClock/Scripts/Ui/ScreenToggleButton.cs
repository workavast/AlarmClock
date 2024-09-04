using Avastrad.UI.UiSystem;
using UnityEngine;
using UnityEngine.UI;

namespace AlarmClock.Scripts.Ui
{
    [RequireComponent(typeof(Button))]
    public class ScreenToggleButton : MonoBehaviour
    {
        [SerializeField] private ScreenType screenType;

        private ScreensController _screensController;
        
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(ToggleScreen);
            _screensController = FindObjectOfType<ScreensController>();
        }

        private void ToggleScreen() 
            => _screensController.SetScreen(screenType);
    }
}