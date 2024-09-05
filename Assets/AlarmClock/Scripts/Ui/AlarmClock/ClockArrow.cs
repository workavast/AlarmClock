using UnityEngine;
using UnityEngine.EventSystems;

namespace AlarmClock.Scripts.Ui.AlarmClock
{
    public abstract class ClockArrow : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        protected PrepareAlarmClockProvider PrepareAlarmClockProvider;
        protected int PrevFullSeconds;
        protected bool IsDrag;
        
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
            var angleSecondsCount = GetSecondsCount(newAngle);//number of seconds in angle (angle in diapason (-180; 180)),
                                                              //so it cant be more then half of full seconds count)

            var fullSecondsCount = GetFullSecondsCount(dragDir, angleSecondsCount);
            if (fullSecondsCount == PrevFullSeconds)
                return;

            newAngle = RoundAngle(angleSecondsCount);
            if (dragDir.x > 0)
                newAngle *= -1;
            
            var currentEulerAngles = transform.eulerAngles;
            transform.rotation = Quaternion.Euler(currentEulerAngles.x, currentEulerAngles.y, newAngle);

            var secondsDifference = CalculateSecondsDifference(fullSecondsCount, PrevFullSeconds);
            
            PrevFullSeconds = fullSecondsCount;
            
            ApplyTime(secondsDifference);
        }
        
        protected abstract void TryUpdateView();
        
        protected float RoundAngle(int stepsCount)
        {
            var angleStep = (float)360 / SecondsCount;
            var roundedAngle = angleStep * stepsCount;
            return roundedAngle;
        }
        
        private int GetSecondsCount(float angle)
        {
            var angleStep = (float)360 / SecondsCount;
            
            var stepsCount = (int)(angle / angleStep);
            
            var remainderAngle = angle % angleStep;
            if (remainderAngle / angleStep >= 0.5f) 
                stepsCount += 1;

            return stepsCount;   
        }

        private int GetFullSecondsCount(Vector2 dragDir, int angleSecondsCount)
        {
            var fullSecondsCount = 0;
            if (dragDir.x < 0)
                fullSecondsCount = SecondsCount - angleSecondsCount;
            else
                fullSecondsCount = angleSecondsCount;

            if (fullSecondsCount == SecondsCount) 
                fullSecondsCount = 0;

            return fullSecondsCount;
        }

        private int CalculateSecondsDifference(int fullSecondsCount, int prevFullSeconds)
        {
            var secondsDifference = fullSecondsCount - prevFullSeconds;
            if (secondsDifference > SecondsCount / 2)
                secondsDifference = -(SecondsCount - secondsDifference);
            if (secondsDifference < -SecondsCount / 2)
                secondsDifference = SecondsCount + secondsDifference;

            return secondsDifference;
        }
        
        private void ApplyTime(int seconds)
        {
            PrepareAlarmClockProvider.PreparedAlarmTime.ChangeSeconds(seconds);
        }
    }
}