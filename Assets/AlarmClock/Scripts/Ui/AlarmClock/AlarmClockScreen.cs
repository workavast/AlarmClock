namespace AlarmClock.Scripts.Ui.AlarmClock
{
    public class AlarmClockScreen : UiScreenBase
    {
        public override void Initialize()
        {
            var alarmViewManagers = GetComponentsInChildren<AlarmViewAndInputManager>(true);
            foreach (var alarmViewManager in alarmViewManagers)
                alarmViewManager.Initialize();
        }
    }
}