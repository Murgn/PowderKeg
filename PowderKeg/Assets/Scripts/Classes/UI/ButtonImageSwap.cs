using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Murgn
{
    public class ButtonImageSwap : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private Sprite normalImage;
        [SerializeField] private Sprite pressedImage;

        public void OnPointerDown(PointerEventData eventData) => image.sprite = pressedImage;

        public void OnPointerUp(PointerEventData eventData) => image.sprite = normalImage;
    }   
}