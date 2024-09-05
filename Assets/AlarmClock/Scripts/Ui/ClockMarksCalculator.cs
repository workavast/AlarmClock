using UnityEngine;

namespace AlarmClock.Scripts.Ui
{
    public class ClockMarksCalculator : MonoBehaviour
    {
        [SerializeField] private Transform[] marks;

        [ContextMenu("Calculate Marks Positions")]
        private void CalculateMarksPositions()
        {
            var marksCount = marks.Length;
            var step = 360 / marksCount;
            var currentAngleValue = 0f;
            
            foreach (var mark in marks)
            {
                var eulerAngles = mark.eulerAngles;
                mark.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, currentAngleValue);
                currentAngleValue += step;
            }
        }
    }
}