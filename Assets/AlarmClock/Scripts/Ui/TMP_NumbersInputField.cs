using TMPro;

namespace AlarmClock.Scripts.Ui
{
    public class TMP_NumbersInputField : TMP_InputField
    {
        protected override bool IsValidChar(char c) 
            => char.IsNumber(c);
    }
}