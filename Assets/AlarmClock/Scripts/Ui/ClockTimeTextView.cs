using TMPro;
using UnityEngine;

namespace AlarmClock.Scripts.Ui
{
    [RequireComponent(typeof(TMP_Text))]
    public class ClockTimeTextView : MonoBehaviour
    {
        private ClockTimeProvider _clockTimeProvider;
        private TMP_Text _text;
        
        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _clockTimeProvider = FindObjectOfType<ClockTimeProvider>();
        }

        private void OnEnable()
        {
            _clockTimeProvider.ClockTime.OnTick += UpdateView;
            UpdateView();
        }

        private void OnDisable() 
            => _clockTimeProvider.ClockTime.OnTick -= UpdateView;

        private void OnDestroy() 
            => _clockTimeProvider.ClockTime.OnTick -= UpdateView;

        private void UpdateView() 
            => _text.text = _clockTimeProvider.ClockTime.ToString();
    }
}