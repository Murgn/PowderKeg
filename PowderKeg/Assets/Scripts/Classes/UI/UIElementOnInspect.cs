using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Murgn
{
    public class UIElementOnInspect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
        [SerializeField] private string elementName;
        private UIManager uiManager;
        
        private void Start() => uiManager = UIManager.instance;
        public void OnPointerEnter(PointerEventData eventData) => uiManager.mouseOverButton = elementName;
        public void OnPointerExit(PointerEventData eventData) => uiManager.mouseOverButton = string.Empty;
        private void OnDisable() => uiManager.mouseOverButton = string.Empty;
    }   
}