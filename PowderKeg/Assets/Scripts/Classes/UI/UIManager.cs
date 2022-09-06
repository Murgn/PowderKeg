using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Murgn
{
    public class UIManager : MonoBehaviour
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

        private List<Vector2> startingTextPositions = new();
        
        // Inspect Mode
        private bool inspectMode;
        private const string defaultTitle = "Powder<color=#464B53>Keg";

        private ParticleManager particleManager;
        private ParticlePlacer particlePlacer;
        private Camera mainCamera;

        private void Start()
        {
            particleManager = ParticleManager.instance;
            particlePlacer = ParticlePlacer.instance;
            MenuButtonStart();
        }

        private void Update()
        {
            PowderCounter();
            FpsCounter();
            BrushText();
            PausedText();
            CursorUpdate();
            InspectUpdate();
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
        
        #region Menu Buttons

        private void MenuButtonStart()
        {
            for (int i = 0; i < menuButtons.Length; i++)
            {
                // Have to pass i into a variable because by the time AddListener is called, its already been changed
                int i2 = i;
                
                startingTextPositions.Add(menuButtons[i2].text.rectTransform.anchoredPosition);

                menuButtons[i2].text.rectTransform.sizeDelta = new Vector2(50, 50);

                switch (menuButtons[i2].menuButtonType)
                {
                    case MenuButtonType.Inspect:
                        menuButtons[i2].button.onClick.AddListener(() => InspectButton(i2));
                        break;
                    case MenuButtonType.Menu:
                        menuButtons[i2].button.onClick.AddListener(() => MenuButton(i2));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        private void InspectButton(int i)
        {
            // Resets all button images
            for (int i2 = 0; i2 < menuButtons.Length; i2++)
            {
                menuButtons[i2].button.image.sprite = normalButton;
                menuButtons[i2].outline.sprite = normalButtonOutline;
                menuButtons[i2].text.rectTransform.anchoredPosition = startingTextPositions[i2];
            }
            
            // Sets current button to pressed
            if (!inspectMode)
            {
                menuButtons[i].button.image.sprite = pressedButton;
                menuButtons[i].outline.sprite = pressedButtonOutline;
                menuButtons[i].text.rectTransform.anchoredPosition = menuButtons[i].textClickedPosition;
            }

            inspectMode = !inspectMode;
        }

        private void InspectUpdate()
        {
            if (!inspectMode) return;

            ParticleId currentId = particlePlacer.GetMouseOverParticleId();
            Debug.Log(currentId);
            titleText.text = currentId != ParticleId.Air && currentId != ParticleId.Null ? particlePlacer.GetMouseOverParticleId().ToString() : defaultTitle;
        }
        
        private void MenuButton(int i)
        {
            // Resets all button images
            for (int i2 = 0; i2 < menuButtons.Length; i2++)
            {
                menuButtons[i2].button.image.sprite = normalButton;
                menuButtons[i2].outline.sprite = normalButtonOutline;
                menuButtons[i2].text.rectTransform.anchoredPosition = startingTextPositions[i2];
            }
            
            // Sets current button to pressed
            menuButtons[i].button.image.sprite = pressedButton;
            menuButtons[i].outline.sprite = pressedButtonOutline;
            menuButtons[i].text.rectTransform.anchoredPosition = menuButtons[i].textClickedPosition;
        }
        
        #endregion
    }   
}