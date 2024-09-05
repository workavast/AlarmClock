namespace AlarmClock.Scripts.Ui
{
    public class AlarmNotificationScreen : UiScreenBase
    {
        private static IAlarmClockProvider AlarmClockProvider => AppData.AlarmClockProvider;

        public override void Initialize() 
            => AlarmClockProvider.OnAlarm += Show;

        private void OnDestroy() 
            => AlarmClockProvider.OnAlarm -= Show;
    }
}