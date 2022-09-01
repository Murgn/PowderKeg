using UnityEngine;

namespace Murgn
{
    [ExecuteInEditMode]
    public class DoodleText : MonoBehaviour
    {
        [SerializeField] private Material mat;

        [SerializeField] private Vector4 DoodleMaxOffset;
        [SerializeField] private float DoodleFrameTime;
        [SerializeField] private int DoodleFrameCount;
        [SerializeField] private Vector4 DoodleNoiseScale;

        [SerializeField] private bool DoodleOn;
        private static readonly int MaxOffset = Shader.PropertyToID("_DoodleMaxOffset");
        private static readonly int FrameTime = Shader.PropertyToID("_DoodleFrameTime");
        private static readonly int FrameCount = Shader.PropertyToID("_DoodleFrameCount");
        private static readonly int NoiseScale = Shader.PropertyToID("_DoodleNoiseScale");

        private void Update() => SetAll();

        private void SetAll()
        {
            mat.SetVector(MaxOffset, DoodleMaxOffset);
            mat.SetFloat(FrameTime, DoodleFrameTime);
            mat.SetInt(FrameCount, DoodleFrameCount);
            mat.SetVector(NoiseScale, DoodleNoiseScale);
            
            if(DoodleOn)
                Shader.EnableKeyword("DOODLE_ON");
            else
                Shader.DisableKeyword("DOODLE_ON");
        }
    }
}