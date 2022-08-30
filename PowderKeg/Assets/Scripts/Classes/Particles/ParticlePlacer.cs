using System;
using Murgn.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Murgn
{
    public class ParticlePlacer : Singleton<ParticlePlacer>
    {
        [HideInInspector] public Vector2 maxScreenPos;
        [HideInInspector] public Vector2 minScreenPos;
        [SerializeField] private Particle selectedParticle = ParticleTypes.Stone;
        // 0 is 1 pixel
        [SerializeField] private float brushRadius = 10;
        private ParticleManager particleManager;
        private Vector2Int mousePos;
        private Camera mainCamera;

        private void Start()
        {
            particleManager = ParticleManager.instance;
            mainCamera = Camera.main;
        }

        private void Update()
        {
            // I need to fix this at some point
            Vector2 screenMousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - (Vector3)particleManager.offset;
            mousePos = new Vector2Int(Mathf.FloorToInt(screenMousePos.x),
                                      Mathf.FloorToInt(screenMousePos.y));

            if (Mouse.current.scroll.ReadValue().y > 0)
                brushRadius++;
            if (Mouse.current.scroll.ReadValue().y < 0)
                brushRadius--;

            brushRadius = Mathf.Clamp(brushRadius, 0, 100);

            // If mousepos is within screenPos, this lets me disable drawing under the sidebar
            if(minScreenPos.x <= screenMousePos.x && screenMousePos.x <= maxScreenPos.x && minScreenPos.y <= screenMousePos.y && screenMousePos.y <= maxScreenPos.y)
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
                                    particleManager.PlaceParticle(new Vector2Int(mousePos.x + x, mousePos.y + y),
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
                                particleManager.PlaceParticle(new Vector2Int(mousePos.x + x, mousePos.y + y),
                                    ParticleTypes.Air, true);
                            }
                        }
                    }
                    //EventManager.Render?.Invoke();
                }
            }

            // Ill remove this eventually, its nice for debugging rn
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
                selectedParticle = ParticleTypes.Stone;
            else if (Keyboard.current.digit2Key.wasPressedThisFrame)
                selectedParticle = ParticleTypes.Dirt;
            else if (Keyboard.current.digit3Key.wasPressedThisFrame)
                selectedParticle = ParticleTypes.Sand;
            else if (Keyboard.current.digit4Key.wasPressedThisFrame)
                selectedParticle = ParticleTypes.Water;
            else if (Keyboard.current.digit5Key.wasPressedThisFrame)
                selectedParticle = ParticleTypes.Steam;
            else if (Keyboard.current.digit6Key.wasPressedThisFrame)
                selectedParticle = ParticleTypes.Fire;
            else if (Keyboard.current.digit7Key.wasPressedThisFrame)
                selectedParticle = ParticleTypes.Wood;
            else if (Keyboard.current.digit8Key.wasPressedThisFrame)
                selectedParticle = ParticleTypes.Oil;
        }

        public void ChangeParticle(ParticleId particleId)
        {
            switch (particleId)
            {
                case ParticleId.Stone:
                    selectedParticle = ParticleTypes.Stone;
                    return;
                
                case ParticleId.Dirt:
                    selectedParticle = ParticleTypes.Dirt;
                    return;
                
                case ParticleId.Sand:
                    selectedParticle = ParticleTypes.Sand;
                    return;
                
                case ParticleId.Water:
                    selectedParticle = ParticleTypes.Water;
                    return;
                
                case ParticleId.Steam:
                    selectedParticle = ParticleTypes.Steam;
                    return;
                
                case ParticleId.Fire:
                    selectedParticle = ParticleTypes.Fire;
                    return;
                
                case ParticleId.Wood:
                    selectedParticle = ParticleTypes.Wood;
                    return;
                
                case ParticleId.Oil:
                    selectedParticle = ParticleTypes.Oil;
                    return;
                
                default:
                    Debug.LogError("Havent assigned the <b>ParticleId</b> to a <b>ParticleType</b> in the <b>ParticlePlacer</b>!");
                    return;
            }
        }
    }
}