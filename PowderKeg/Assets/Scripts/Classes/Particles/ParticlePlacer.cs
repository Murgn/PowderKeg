using System;
using Murgn.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Murgn
{
    public class ParticlePlacer : Singleton<ParticlePlacer>
    {
        [SerializeField] private Particle selectedParticle = ParticleTypes.Stone;
        // 0 is 1 pixel
        public float brushRadius = 10;
        [SerializeField] private Canvas canvas;
        private Vector2Int flooredMousePos;

        private Vector2 arraySize;
        private Vector2 rectangleSize;
        private Vector2 relativePosition;

        [SerializeField] private bool withinRect;

        private ParticleManager particleManager;
        private ParticleRenderer particleRenderer;
        private UIManager uiManager;
        
        private void Start()
        {
            particleManager = ParticleManager.instance;
            particleRenderer = ParticleRenderer.instance;
            uiManager = UIManager.instance;
        }

        private void Update()
        {
            UpdateImageTransformations();
            Vector2 mousePos = new Vector2(relativePosition.x * arraySize.x / rectangleSize.x,
                relativePosition.y * arraySize.y / rectangleSize.y);
            
            flooredMousePos = new Vector2Int(Mathf.FloorToInt(mousePos.x), Mathf.FloorToInt(mousePos.y));

            if (Mouse.current.scroll.ReadValue().y > 0)
                brushRadius++;
            if (Mouse.current.scroll.ReadValue().y < 0)
                brushRadius--;

            brushRadius = Mathf.Clamp(brushRadius, 0, 99);

            if (withinRect && Mouse.current.middleButton.wasPressedThisFrame)
                uiManager.ClickSidebarButton(GetMouseOverParticleId());
            
            
            if (withinRect && !uiManager.inspectMode && !uiManager.inMenu)
            {
                if (Mouse.current.leftButton.isPressed)
                {
                    int size = (int)brushRadius + 1;

                    for (int x = -size; x < size; x++)
                    {
                        for (int y = -size; y < size; y++)
                        {
                            if (x * x + y * y <= brushRadius * brushRadius)
                            {
                                // Makes sand dispersal more random.
                                bool rand = Random.Range(0.0f, 1.0f) > selectedParticle.dispersalChance;
                                bool place = rand ? true : false;

                                if (place || brushRadius <= 1)
                                    //inside or on the rim
                                    particleManager.PlaceParticle(new Vector2Int(flooredMousePos.x + x, flooredMousePos.y + y),
                                        selectedParticle, false, true);
                            }
                        }
                    }

                    //EventManager.Render?.Invoke();
                }
                else if (Mouse.current.rightButton.isPressed)
                {
                    int size = (int)brushRadius + 1;
                    for (int x = -size; x < size; x++)
                    {
                        for (int y = -size; y < size; y++)
                        {
                            if (x * x + y * y <= brushRadius * brushRadius)
                            {
                                //inside or on the rim
                                particleManager.PlaceParticle(new Vector2Int(flooredMousePos.x + x, flooredMousePos.y + y),
                                    ParticleTypes.Air, true);
                            }
                        }
                    }
                    //EventManager.Render?.Invoke();
                }
            }
        }

        public void ChangeParticle(ParticleId particleId) => selectedParticle = ParticleLookup.IdToParticle[particleId];

        private void UpdateImageTransformations()
        {
            arraySize = new Vector2(particleManager.width, particleManager.height);
            
            Vector2 rectanglePos = particleRenderer.image.rectTransform.position / canvas.scaleFactor;
            rectangleSize = particleRenderer.image.rectTransform.sizeDelta;
            
            Vector2 mousePos = Mouse.current.position.ReadValue() / canvas.scaleFactor;
            Vector2 offsettedMouse = mousePos - (new Vector2(-rectangleSize.x, -rectangleSize.y) / 2);
            
            // Check if mouse is within rect
            if (rectanglePos.x - (rectangleSize.x / 2) <= mousePos.x && mousePos.x <= rectanglePos.x + (rectangleSize.x / 2) &&
                rectanglePos.y - (rectangleSize.y / 2) <= mousePos.y && mousePos.y <= rectanglePos.y + (rectangleSize.y / 2))
                withinRect = true;
            else
                withinRect = false;

            // Position relative to rectangle
            relativePosition = new Vector2(offsettedMouse.x - rectanglePos.x, offsettedMouse.y - rectanglePos.y);
        }

        public ParticleId GetMouseOverParticleId() => particleManager.GetParticle(flooredMousePos).id;
    }
}