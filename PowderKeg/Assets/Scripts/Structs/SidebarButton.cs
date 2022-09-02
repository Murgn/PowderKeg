using System;
using UnityEngine;
using UnityEngine.UI;

namespace Murgn
{
    [Serializable]
    public struct SidebarButton
    {
        public Button button;
        public Image outline;
        public ParticleId particleId;
        public Color color;
        public bool pressed;
    }
}