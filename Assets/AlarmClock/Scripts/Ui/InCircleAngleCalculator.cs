using UnityEngine;

namespace AlarmClock.Scripts.Ui
{
    public class InCircleAngleCalculator : MonoBehaviour
    {
        [SerializeField] private Transform[] marks;

        [ContextMenu("Calculate Positions")]
        private void CalculatePositions()
        {
            var marksCount = marks.Length;
            var angleStep = 360 / marksCount;
            var currentAngleValue = 0f;
            
            foreach (var mark in marks)
            {
                var eulerAngles = mark.eulerAngles;
                mark.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, currentAngleValue);
                currentAngleValue += angleStep;
            }
        }
    }
}