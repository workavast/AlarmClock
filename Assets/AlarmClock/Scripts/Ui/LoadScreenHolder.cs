using UnityEngine;

namespace AlarmClock.Scripts.Ui
{
    public class LoadScreenHolder : MonoBehaviour
    {
        [SerializeField] private GameObject loadScreen; 
        
        private ClockTimeProvider _clockTimeProvider;
        
        private void Awake()
        {
            loadScreen.SetActive(true);
            _clockTimeProvider = FindObjectOfType<ClockTimeProvider>();
            _clockTimeProvider.OnInitialized += Hide;
        }

        private void Hide()
        {
            _clockTimeProvider.OnInitialized -= Hide;
            loadScreen.SetActive(false);
        }
    }
}