using TMPro;
using UnityEngine;

namespace Murgn
{
    public class FpsCounter : MonoBehaviour
	{
        [SerializeField] private TextMeshProUGUI fpsText;
        private float pollingTime = 0.25f;
        private float time;
        private int frameCount;

        private void Update()
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
    }   
}