using System.Collections.Generic;
using UnityEngine;


namespace Murgn
{
    public class SidebarController : MonoBehaviour
    {
        [NonReorderable]
        [SerializeField] private SidebarButton[] sidebarButtons;

        private List<ButtonImageSwap> buttonImageSwaps = new();

        [SerializeField] private Sprite normalButton;
        [SerializeField] private Sprite pressedButton;
        [SerializeField] private Sprite normalButtonOutline;
        [SerializeField] private Sprite pressedButtonOutline;
        
        private ParticlePlacer particlePlacer;
        private UIManager uiManager;

        private void Start()
        {
            particlePlacer = ParticlePlacer.instance;
            uiManager = UIManager.instance;

            for (int i = 0; i < sidebarButtons.Length; i++)
            {
                // Have to pass i into a variable because by the time AddListener is called, its already been changed
                int i2 = i;
                sidebarButtons[i].button.image.color = sidebarButtons[i].color;
                 
                buttonImageSwaps.Add(sidebarButtons[i].button.GetComponent<ButtonImageSwap>());
                buttonImageSwaps[i].thisSidebarButton = sidebarButtons[i];
                
                sidebarButtons[i2].button.onClick.AddListener(() => SetParticle(i2, sidebarButtons[i2].particleId));
                sidebarButtons[i2].button.onClick.AddListener(() => uiManager.ResetButtons());
            }
        }

        public void SetParticle(int i, ParticleId particleId)
        {
            for (int i2 = 0; i2 < sidebarButtons.Length; i2++)
            {
                sidebarButtons[i2].button.image.sprite = normalButton;
                sidebarButtons[i2].outline.sprite = normalButtonOutline;
                sidebarButtons[i2].pressed = false;
                buttonImageSwaps[i2].thisSidebarButton = sidebarButtons[i2];
            }

            sidebarButtons[i].button.image.sprite = pressedButton;
            sidebarButtons[i].outline.sprite = pressedButtonOutline;
            sidebarButtons[i].pressed = true;
            buttonImageSwaps[i].thisSidebarButton = sidebarButtons[i];
            particlePlacer.ChangeParticle(particleId);
        }

        public void ClickButton(ParticleId particleId)
        {
            for (int i = 0; i < sidebarButtons.Length; i++)
            {
                if(sidebarButtons[i].particleId == particleId)
                    sidebarButtons[i].button.onClick.Invoke();
            }
        }

        public void ResetButtons()
        {
            for (int i = 0; i < sidebarButtons.Length; i++)
            {
                sidebarButtons[i].button.image.sprite = normalButton;
                sidebarButtons[i].outline.sprite = normalButtonOutline;
                sidebarButtons[i].pressed = false;
                buttonImageSwaps[i].thisSidebarButton = sidebarButtons[i];
            }
        }
        
    }   
}