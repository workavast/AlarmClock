using UnityEngine;

namespace AlarmClock.Scripts.Ui
{
    public class UiScreenOrientationUpdater : MonoBehaviour
    {
        private UiScreenBase[] _screens;
        private ScreenOrientation _prevScreenOrientation;

        private void Awake()
        {
            _screens = GetComponentsInChildren<UiScreenBase>();
        }

        private void Start()
        {
            _prevScreenOrientation = Screen.orientation;
            UpdateScreensOrientation(_prevScreenOrientation);
        }
        
        private void Update()
        {
            if (Screen.orientation != _prevScreenOrientation)
            {
                _prevScreenOrientation = Screen.orientation;
                UpdateScreensOrientation(_prevScreenOrientation);
            }
        }

        private void UpdateScreensOrientation(ScreenOrientation newScreenOrientation)
        {
            foreach (var screen in _screens) 
                screen.ChangeOrientation(newScreenOrientation);
        }
    }
}