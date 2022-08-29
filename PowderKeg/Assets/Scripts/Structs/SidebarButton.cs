using System;
using UnityEngine;
using UnityEngine.UI;

namespace Murgn
{
    [Serializable]
    public struct SidebarButton
    {
        public Button button;
        public ParticleId particleId;
        public Color color;
    }
}