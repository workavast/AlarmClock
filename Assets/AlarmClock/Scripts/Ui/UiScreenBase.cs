using UnityEngine;

namespace AlarmClock.Scripts.Ui
{
    public abstract class UiScreenBase : MonoBehaviour
    {
        [SerializeField] private Canvas portraitCanvas;
        [SerializeField] private Canvas landscapeCanvas;
        
        public void ChangeOrientation(ScreenOrientation newScreenOrientation)
        {
            portraitCanvas.gameObject.SetActive(false);
            landscapeCanvas.gameObject.SetActive(false);
            
            switch (newScreenOrientation)
            {
                case ScreenOrientation.Portrait:
                    portraitCanvas.gameObject.SetActive(true);
                    break;
                case ScreenOrientation.PortraitUpsideDown:
                    portraitCanvas.gameObject.SetActive(true);
                    break;
                case ScreenOrientation.LandscapeLeft:
                    landscapeCanvas.gameObject.SetActive(true);
                    break;
                case ScreenOrientation.LandscapeRight:
                    landscapeCanvas.gameObject.SetActive(true);
                    break;
                default:
                    portraitCanvas.gameObject.SetActive(true);
                break;
            }
        }
    }
}