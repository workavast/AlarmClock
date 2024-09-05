namespace AlarmClock.Scripts.Ui.AlarmClock
{
    public class AlarmClockScreen : UiScreenBase
    {
        public override void Initialize()
        {
            var alarmViewManagers = GetComponentsInChildren<AlarmViewManager>();
            foreach (var alarmViewManager in alarmViewManagers)
                alarmViewManager.Initialize();
        }
    }
}