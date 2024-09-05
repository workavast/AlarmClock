using System;
using UnityEngine;

namespace AlarmClock.Scripts
{
    public class UpdateProvider : MonoBehaviour, IUpdateProvider
    {
        private static UpdateProvider _instance;
        
        public event Action OnUpdate;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            AppData.SetUpdater(this);
            DontDestroyOnLoad(gameObject);
        }

        private void Update() 
            => OnUpdate?.Invoke();
    }
}