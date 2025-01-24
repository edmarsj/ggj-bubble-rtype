using Game.Sounds;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Menu
{
    public class Button_base : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            //Call
            Sound_system.Create_sound("Button_confirm", 1f);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //Call
            Sound_system.Create_sound("Cursor_select", 1f);
        }
    }
}