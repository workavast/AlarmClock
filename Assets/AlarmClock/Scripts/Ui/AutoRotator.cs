using UnityEngine;

namespace AlarmClock.Scripts.Ui
{
    public class AutoRotator : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 1;

        private void Update() 
            => transform.Rotate(Vector3.back * (rotationSpeed * Time.deltaTime));
    }
}