using System;
using TMPro;
using UnityEngine;

namespace Murgn
{
    public class UIManager : MonoBehaviour
	{
        [SerializeField] private TextMeshProUGUI powderText;
        [SerializeField] private TextMeshProUGUI fpsText;
        [SerializeField] private TextMeshProUGUI brushText;
        [SerializeField] private TextMeshProUGUI pausedText;

        private float pollingTime = 0.25f;
        private float time;
        private int frameCount;

        private float dotsTimer = 0.5f;
        private float dotsCounter;
        private int pausedDots;

        private ParticleManager particleManager;
        private ParticlePlacer particlePlacer;

        private void Start()
        {
            particleManager = ParticleManager.instance;
            particlePlacer = ParticlePlacer.instance;
        }

        private void Update()
        {
            PowderCounter();
            FpsCounter();
            BrushText();
            PausedText();
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
    }   
}