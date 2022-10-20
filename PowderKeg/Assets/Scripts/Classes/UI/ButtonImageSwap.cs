using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Murgn
{
    public class ButtonImageSwap : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public SidebarButton thisSidebarButton;
        public MenuButton thisMenuButton;
        [SerializeField] private Image image;
        [SerializeField] private Sprite normalImage;
        [SerializeField] private Sprite pressedImage;

        private RectTransform rectTransform;
        private UIManager uiManager;

        private void Start()
        {
            rectTransform = (RectTransform)transform;
            uiManager = UIManager.instance;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                image.sprite = pressedImage;
                if (thisMenuButton.button != null)
                    thisMenuButton.text.rectTransform.anchoredPosition = thisMenuButton.textClickedPosition;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (thisSidebarButton.button != null)
                {
                    if((rectTransform.rect.Contains(eventData.position) && thisSidebarButton.pressed) || !thisSidebarButton.pressed)
                        image.sprite = normalImage;
                }

                if (thisMenuButton.button != null)
                {
                    if((rectTransform.rect.Contains(eventData.position) && thisMenuButton.pressed) || !thisMenuButton.pressed)
                    {
                        image.sprite = normalImage;
                        thisMenuButton.text.rectTransform.anchoredPosition = thisMenuButton.textStartPosition;
                    }
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (thisSidebarButton.button != null)
            {
                uiManager.mouseOverButton = thisSidebarButton.particleId.ToString();
            }

            if (thisMenuButton.button != null)
            {
                uiManager.mouseOverButton = thisMenuButton.menuButtonType + " <color=#464B53>Button";
            }
        }

        public void OnPointerExit(PointerEventData eventData) => uiManager.mouseOverButton = string.Empty;
    }   
}