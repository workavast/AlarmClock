namespace AlarmClock.Scripts.Ui.AlarmClock
{
    public class AlarmClockScreen : UiScreenBase
    {
        public override void Initialize()
        {
            var alarmViewManagers = GetComponentsInChildren<AlarmViewManager>(true);
            foreach (var alarmViewManager in alarmViewManagers)
                alarmViewManager.Initialize();
        }
    }
}