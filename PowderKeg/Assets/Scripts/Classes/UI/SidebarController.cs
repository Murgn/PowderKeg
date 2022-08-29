using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Murgn
{
    public class SidebarController : MonoBehaviour
    {
        [NonReorderable]
        [SerializeField] private SidebarButton[] sidebarButtons;

        [SerializeField] private Sprite normalButton;
        [SerializeField] private Sprite pressedButton;
        
        private bool sidebarEnabled = false;
        private ParticlePlacer particlePlacer;
        private Animator animator;
        private CanvasScaler canvasScaler;
        private RectTransform rectTransform;

        private void Start()
        {
            particlePlacer = ParticlePlacer.instance;
            animator = GetComponent<Animator>();
            canvasScaler = transform.GetComponentInParent<CanvasScaler>();
            rectTransform = (RectTransform)transform;

            for (int i = 0; i < sidebarButtons.Length; i++)
            {
                // Have to pass i into a variable because by the time AddListener is called, its already been changed
                int i2 = i;
                sidebarButtons[i].button.image.color = sidebarButtons[i].color;
                sidebarButtons[i2].button.onClick.AddListener(() => SetParticle(i2, sidebarButtons[i2].particleId));
            }
        }
        
        private void Update()
        {
            if (Keyboard.current.tabKey.wasPressedThisFrame)
            {
                sidebarEnabled = !sidebarEnabled;
                
                animator.CrossFade(sidebarEnabled ? "SlideIn" : "SlideOut", 0);
            }

            Vector2 padding = new Vector2(5, 10);
            particlePlacer.maxScreenPos = canvasScaler.referenceResolution + padding;
            particlePlacer.minScreenPos = sidebarEnabled ? new Vector2(rectTransform.sizeDelta.x - padding.x, 0 - padding.y) : Vector2.zero - padding;
        }
        
        public void SetParticle(int i, ParticleId particleId)
        {
            for (int i2 = 0; i2 < sidebarButtons.Length; i2++)
                sidebarButtons[i2].button.image.sprite = normalButton;

            sidebarButtons[i].button.image.sprite = pressedButton;
            particlePlacer.ChangeParticle(particleId);
        }
        
    }   
}