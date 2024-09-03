using TMPro;
using UnityEngine;

namespace AlarmClock.Scripts
{
    [RequireComponent(typeof(TMP_Text))]
    public class ClockTimeTextView : MonoBehaviour
    {
        private ClockTimeProvider _clockTimeProvider;
        private TMP_Text _text;
        
        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            _clockTimeProvider = FindObjectOfType<ClockTimeProvider>();
            _clockTimeProvider.ClockTime.OnTick += UpdateView;
        }

        private void UpdateView()
        {
            _text.text = _clockTimeProvider.ClockTime.ToString();
        }
    }
}