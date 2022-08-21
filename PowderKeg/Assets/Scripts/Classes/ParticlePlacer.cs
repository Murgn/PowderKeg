using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Murgn
{
    public class ParticlePlacer : MonoBehaviour
    {
        [SerializeField] private Particle selectedParticle = ParticleTypes.Block;
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
            Vector2 screenMousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - (Vector3)particleManager.offset;
            mousePos = new Vector2Int(Mathf.FloorToInt(screenMousePos.x),
                                      Mathf.FloorToInt(screenMousePos.y));

            if (Mouse.current.scroll.ReadValue().y > 0)
                brushRadius++;
            if (Mouse.current.scroll.ReadValue().y < 0)
                brushRadius--;

            brushRadius = Mathf.Clamp(brushRadius, 0, 100);
            
            if (Mouse.current.leftButton.isPressed)
            {
                int size = (int)brushRadius + 1;

                for(int x=-size; x<size; x++)
                {
                    for(int y=-size; y<size; y++)
                    {
                        if(x*x+y*y <= brushRadius*brushRadius)
                        {
                            // Makes sand dispersal more random.
                            bool rand = Random.Range(0.0f, 1.0f) > selectedParticle.dispersalChance;
                            bool place = rand ? true : false;

                            if (place)
                                //inside or on the rim
                                particleManager.PlaceParticle(new Vector2Int(mousePos.x + x, mousePos.y + y),
                                    selectedParticle);
                        }
                    }
                }
                
                EventManager.Render?.Invoke();
            }
            else if (Mouse.current.rightButton.isPressed)
            {
                int size = (int)brushRadius + 1;
                for(int x=-size; x<size; x++)
                {
                    for(int y=-size; y<size; y++)
                    {
                        if(x*x+y*y <= brushRadius*brushRadius)
                        {
                            //inside or on the rim
                            particleManager.PlaceParticle(new Vector2Int(mousePos.x + x, mousePos.y + y),
                                ParticleTypes.Air, true);
                        }
                    }
                }
                EventManager.Render?.Invoke();
            }

            if (Keyboard.current.digit1Key.wasPressedThisFrame)
                selectedParticle = ParticleTypes.Block;
            else if (Keyboard.current.digit2Key.wasPressedThisFrame)
                selectedParticle = ParticleTypes.Dirt;
            else if (Keyboard.current.digit3Key.wasPressedThisFrame)
                selectedParticle = ParticleTypes.Sand;
            else if (Keyboard.current.digit4Key.wasPressedThisFrame)
                selectedParticle = ParticleTypes.Water;
            else if (Keyboard.current.digit5Key.wasPressedThisFrame)
                selectedParticle = ParticleTypes.Steam;
        }
    }
}