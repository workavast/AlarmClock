namespace AlarmClock.Scripts
{
    public static class AppData
    {
        private static readonly ClockTimeProvider _clockTimeProvider;
        private static readonly AlarmClockProvider _alarmClockProvider;
        private static readonly PrepareAlarmClockProvider _prepareAlarmClockProvider;
        
        public static IClockTimeProvider ClockTimeProvider => _clockTimeProvider;
        public static IAlarmClockProvider AlarmClockProvider => _alarmClockProvider;
        public static IPrepareAlarmClockProvider PrepareAlarmClockProvider => _prepareAlarmClockProvider;
        
        private static IUpdateProvider _updateProvider;
        
        static AppData()
        {
            _clockTimeProvider = new ClockTimeProvider();
            _alarmClockProvider = new AlarmClockProvider(_clockTimeProvider);
            _prepareAlarmClockProvider = new PrepareAlarmClockProvider();
        }

        public static void SetUpdater(IUpdateProvider updateProvider)
        {
            if (!_updateProvider.IsAnyNull()) 
                _updateProvider.OnUpdate += Update;

            if (!updateProvider.IsAnyNull()) 
                updateProvider.OnUpdate += Update;
        }

        private static void Update() 
            => _clockTimeProvider.Update();
        
        private static bool IsAnyNull<T>(this T value) 
            => value == null || ((value is UnityEngine.Object) && (value as UnityEngine.Object) == null);
    }
}