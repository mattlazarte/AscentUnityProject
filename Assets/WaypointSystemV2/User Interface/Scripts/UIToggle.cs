using Ascent.Managers.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ascent.UI
{
    public class UIToggle : Toggle, ISelectHandler
    {
        public override void OnPointerDown(PointerEventData eventData)
        {
            //SoundFxsManager.instance.PlayOneShot2D(SoundFx.PlasmaGunShot);
            base.OnPointerDown(eventData);
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            //SoundFxsManager.instance.PlayOneShot2D(SoundFx.PlasmaGunShot);
            base.OnSubmit(eventData);
        }
    }
}