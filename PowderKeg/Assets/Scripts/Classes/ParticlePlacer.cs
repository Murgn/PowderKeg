using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Murgn
{
    public class ParticlePlacer : MonoBehaviour
    {
        [SerializeField] private ParticleId selectedParticle;
        // 0 is 1 pixel
        [SerializeField] private float brushRadius = 10;
        [SerializeField] private bool disperseBrush;
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
            
            if (Mouse.current.leftButton.isPressed)
            {
                int size = (int)brushRadius + 1;
                switch (selectedParticle)
                {
                    // Particles that don't use dispersal
                    case ParticleId.Block:
                        for(int x=-size; x<size; x++)
                        {
                            for(int y=-size; y<size; y++)
                            {
                                if(x*x+y*y <= brushRadius*brushRadius)
                                {
                                    //inside or on the rim
                                    particleManager.PlaceParticle(new Vector2Int(mousePos.x + x, mousePos.y + y),
                                        selectedParticle);
                                }
                            }
                        }
                        break;
                    
                    // Particles that use dispersal
                    default:
                        for(int x=-size; x<size; x++)
                        {
                            for(int y=-size; y<size; y++)
                            {
                                if(x*x+y*y <= brushRadius*brushRadius)
                                {
                                    // Makes sand dispersal more random.
                                    bool rand = Random.Range(0.0f, 1.0f) > 0.9f;
                                    bool place = rand ? true : false;

                                    if (place || !disperseBrush)
                                        //inside or on the rim
                                        particleManager.PlaceParticle(new Vector2Int(mousePos.x + x, mousePos.y + y),
                                            selectedParticle);
                                }
                            }
                        }
                        break;
                }
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
                            particleManager.PlaceParticle(new Vector2Int(mousePos.x + x, mousePos.y + y), ParticleId.Air, true);
                        }
                    }
                }
            }

            if (Keyboard.current.digit1Key.wasPressedThisFrame)
                selectedParticle = ParticleId.Block;
            else if (Keyboard.current.digit2Key.wasPressedThisFrame)
                selectedParticle = ParticleId.Dirt;
            else if (Keyboard.current.digit3Key.wasPressedThisFrame)
                selectedParticle = ParticleId.Sand;
            else if (Keyboard.current.digit4Key.wasPressedThisFrame)
                selectedParticle = ParticleId.Water;
            else if (Keyboard.current.digit5Key.wasPressedThisFrame)
                selectedParticle = ParticleId.Steam;
        }
    }
}