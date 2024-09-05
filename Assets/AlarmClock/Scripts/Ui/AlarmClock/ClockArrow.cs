using UnityEngine;
using UnityEngine.EventSystems;

namespace AlarmClock.Scripts.Ui.AlarmClock
{
    public abstract class ClockArrow : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        protected PrepareAlarmClockProvider PrepareAlarmClockProvider;
        protected bool IsDrag;
        protected int PrevFullSeconds;
        
        protected abstract int SecondsCount { get; }
        public bool Interactable { get; set; } = true;
        
        public void Initialize()
        {
            PrepareAlarmClockProvider = FindObjectOfType<PrepareAlarmClockProvider>();
        }

        private void OnEnable()
        {
            PrepareAlarmClockProvider.PreparedAlarmTime.OnTick += TryUpdateView;
            TryUpdateView();
        }

        private void OnDisable()
        {
            PrepareAlarmClockProvider.PreparedAlarmTime.OnTick -= TryUpdateView;
        }

        public void OnBeginDrag(PointerEventData eventData) 
            => IsDrag = true;

        public void OnEndDrag(PointerEventData eventData) 
            => IsDrag = false;

        public void OnDrag(PointerEventData eventData)
        {
            if (!Interactable)
                return;

            var dragPos = eventData.position;
            var dragDir = (dragPos - (Vector2)transform.position).normalized;
            
            var newAngle = Vector2.Angle(Vector2.up, dragDir);
            var secondsCount = GetSecondsCount(newAngle);
            
            
            var fullSecondsCount = 0;
            if (dragDir.x < 0) 
                fullSecondsCount = SecondsCount - secondsCount;
            else
                fullSecondsCount = secondsCount;

            if (fullSecondsCount == SecondsCount) 
                fullSecondsCount = 0;

            if (fullSecondsCount == PrevFullSeconds)
                return;

            newAngle = GetRoundAngle(secondsCount);
            if (dragDir.x > 0)
                newAngle *= -1;
            var eulerAngles = transform.eulerAngles;
            transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, newAngle);
            
            var secondsDifference = fullSecondsCount - PrevFullSeconds;
            if (secondsDifference > SecondsCount / 2)
                secondsDifference = -(SecondsCount - secondsDifference);
            if (secondsDifference < -SecondsCount / 2)
                secondsDifference = SecondsCount + secondsDifference;
            
            // Debug.Log($"{fullSecondsCount} || {PrevFullSeconds} || {secondsDifference}");
            PrevFullSeconds = fullSecondsCount;
            
            ApplyTime(secondsDifference);
        }
        
        protected abstract void TryUpdateView();
        
        private int GetSecondsCount(float angle)
        {
            var step = (float)360 / SecondsCount;
            
            var stepsCount = (int)(angle / step);
            
            var dif = angle % step;
            if (dif / step >= 0.5f) 
                stepsCount += 1;

            return stepsCount;   
        }
        
        protected float GetRoundAngle(int stepsCount)
        {
            var step = (float)360 / SecondsCount;
            var returnAngle = step * stepsCount;
            return returnAngle;
        }

        private void ApplyTime(int seconds)
        {
            PrepareAlarmClockProvider.PreparedAlarmTime.ChangeSeconds(seconds);
        }
    }
}