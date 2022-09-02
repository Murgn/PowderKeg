using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Murgn
{
    public class ButtonImageSwap : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public SidebarButton thisSidebarButton;
        [SerializeField] private Image image;
        [SerializeField] private Sprite normalImage;
        [SerializeField] private Sprite pressedImage;

        private RectTransform rectTransform;

        private void Start() => rectTransform = (RectTransform)transform;

        public void OnPointerDown(PointerEventData eventData) => image.sprite = pressedImage;

        public void OnPointerUp(PointerEventData eventData)
        {
            if(rectTransform.rect.Contains(eventData.position) && thisSidebarButton.pressed)
                image.sprite = normalImage;
            else if(!thisSidebarButton.pressed)
                image.sprite = normalImage;
        }
    }   
}