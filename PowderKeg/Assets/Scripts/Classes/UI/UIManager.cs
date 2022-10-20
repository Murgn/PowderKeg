using System;
using System.Collections.Generic;
using Murgn.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Murgn
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("Cursor")] 
        [SerializeField] private Image cursor;
        [SerializeField] private Sprite[] cursorSprites;
        
        [Header("Info Bar")]
        [SerializeField] private TextMeshProUGUI powderText;
        [SerializeField] private TextMeshProUGUI fpsText;
        [SerializeField] private TextMeshProUGUI brushText;
        [SerializeField] private TextMeshProUGUI pausedText;
        [SerializeField] private TextMeshProUGUI titleText;

        private const float pollingTime = 0.25f;
        private float time;
        private int frameCount;

        private const float dotsTimer = 0.5f;
        private float dotsCounter;
        private int pausedDots;

        [Header("Menu Buttons")] 
        [NonReorderable] [SerializeField] private MenuButton[] menuButtons;
        
        [SerializeField] private Sprite normalButton;
        [SerializeField] private Sprite pressedButton;
        [SerializeField] private Sprite normalButtonOutline;
        [SerializeField] private Sprite pressedButtonOutline;

        private List<ButtonImageSwap> buttonImageSwaps = new();

        // Inspect Mode
        public bool inspectMode;
        private const string defaultTitle = "Powder<color=#464B53>Keg";
        [HideInInspector] public string mouseOverButton;
        
        // Menu Button
        [HideInInspector] public bool inMenu;

        [Header("Options")] 
        [SerializeField] private TMP_Dropdown backgroundDropdown;
        [NonReorderable] [SerializeField] private BackgroundColor[] backgroundColors;
        [SerializeField] private TextMeshProUGUI simulationSpeedText;

        
        [Header("Miscellaneous")] 
        private ParticleManager particleManager;
        private ParticlePlacer particlePlacer;
        private ParticleRenderer particleRenderer;
        [SerializeField] private SidebarController sidebarController;
        private Camera mainCamera;

        private void Start()
        {
            particleManager = ParticleManager.instance;
            particlePlacer = ParticlePlacer.instance;
            particleRenderer = ParticleRenderer.instance;
            MenuButtonStart();
            BackgroundDropdownSetup();
        }

        private void Update()
        {
            PowderCounter();
            FpsCounter();
            BrushText();
            PausedText();
            CursorUpdate();
            InspectUpdate();
            SimulationSpeedUpdate();

            particleManager.inMenu = inMenu;
        }

        private void CursorUpdate()
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            cursor.transform.position = mousePos;
            cursor.sprite = inspectMode ? cursorSprites[1] : cursorSprites[0];
            Cursor.visible = false;
        }

        private void PowderCounter()
        {
            powderText.text = string.Format("{0:D5} powder", particleManager.maxParticleCount - particleManager.particleCount);
        }

        private void FpsCounter()
        {
            time += Time.unscaledDeltaTime;
            frameCount++;
            if (time >= pollingTime)
            {
                int frameRate = Mathf.RoundToInt(frameCount / time);
                fpsText.text = frameRate.ToString() + " fps";

                time -= pollingTime;
                frameCount = 0;
            }
        }

        private void BrushText()
        {
            brushText.text = (particlePlacer.brushRadius + 1).ToString();
        }
        
        private void PausedText()
        {
            pausedText.gameObject.SetActive(particleManager.paused);

            if (particleManager.paused)
            {
                if (dotsCounter <= Time.time)
                {
                    pausedDots++;
                    dotsCounter = dotsTimer + Time.time;
                }

                if (pausedDots > 3) pausedDots = 0;
            
                string dots = string.Empty;
                for (int i = 0; i < pausedDots; i++)
                    dots += ".";
            
                pausedText.text = "PAUSED" + dots;
            }
        }

        public void ClickSidebarButton(ParticleId particleId) => sidebarController.ClickButton(particleId);
        
        #region Menu Buttons

        private void MenuButtonStart()
        {
            for (int i = 0; i < menuButtons.Length; i++)
            {
                // Have to pass i into a variable because by the time AddListener is called, its already been changed
                int i2 = i;

                menuButtons[i2].textStartPosition = menuButtons[i2].text.rectTransform.anchoredPosition;

                buttonImageSwaps.Add(menuButtons[i2].button.GetComponent<ButtonImageSwap>());
                buttonImageSwaps[i].thisMenuButton = menuButtons[i2];
                
                menuButtons[i2].button.onClick.AddListener(() => sidebarController.ResetButtons());

                switch (menuButtons[i2].menuButtonType)
                {
                    case MenuButtonType.Inspect:
                        menuButtons[i2].button.onClick.AddListener(() => InspectButton(i2));
                        break;
                    case MenuButtonType.Menu:
                        menuButtons[i2].button.onClick.AddListener(() => MenuButton(i2));
                        break;
                    case MenuButtonType.Back:
                        menuButtons[i2].button.onClick.AddListener(() => BackButton(i2));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        private void InspectButton(int i)
        {
            ResetButtons(false);
            
            if (!inspectMode)
                PressButton(i);

            inspectMode = !inspectMode;
        }

        private void InspectUpdate()
        {
            // I might try out having inspect mode on 24/7
            // if (!inspectMode)
            // {
            //     titleText.text = defaultTitle;
            //     return;
            // }

            ParticleId currentId = particlePlacer.GetMouseOverParticleId();
            
            if (currentId != ParticleId.Air && currentId != ParticleId.Null)
                titleText.text = particlePlacer.GetMouseOverParticleId().ToString();
            else if (!string.IsNullOrEmpty(mouseOverButton))
                titleText.text = mouseOverButton;
            else
                titleText.text = defaultTitle;
        }
        
        private void MenuButton(int i)
        {
            ResetButtons();
            PressButton(i);

            inMenu = true;
        }
        
        private void BackButton(int i)
        {
            ResetButtons();
            PressButton(i);

            inMenu = false;
        }
        
        public void ResetButtons(bool resetInspectMode = true)
        {
            inspectMode = !resetInspectMode && inspectMode;
            
            for (int i = 0; i < menuButtons.Length; i++)
            {
                menuButtons[i].button.image.sprite = normalButton;
                menuButtons[i].outline.sprite = normalButtonOutline;
                menuButtons[i].text.rectTransform.anchoredPosition = menuButtons[i].textStartPosition;
                menuButtons[i].pressed = false;
                buttonImageSwaps[i].thisMenuButton = menuButtons[i];
            }
        }

        private void PressButton(int i)
        {
            menuButtons[i].button.image.sprite = pressedButton;
            menuButtons[i].outline.sprite = pressedButtonOutline;
            menuButtons[i].text.rectTransform.anchoredPosition = menuButtons[i].textClickedPosition;
            menuButtons[i].pressed = true;
            buttonImageSwaps[i].thisMenuButton = menuButtons[i];
        }

        #endregion
        
        #region Options Menu

        private void BackgroundDropdownSetup()
        {
            backgroundDropdown.ClearOptions();
            
            List<string> dropOptions = new();   
            
            for (int i = 0; i < backgroundColors.Length; i++)
                dropOptions.Add(backgroundColors[i].colorName);
            
            backgroundDropdown.AddOptions(dropOptions);
            //backgroundDropdown.image.color = backgroundColors[0].color;
            particleRenderer.airColor = backgroundColors[0].color;
        }

        public void BackgroundChange(int i)
        {
            particleRenderer.airColor = backgroundColors[i].color;
            //backgroundDropdown.image.color = backgroundColors[i].color;
        }

        private void SimulationSpeedUpdate() => simulationSpeedText.text = particleManager.updateSpeed[particleManager.currentSpeed].speedText;

        public void Quit() => Application.Quit();

        #endregion
    }   
}