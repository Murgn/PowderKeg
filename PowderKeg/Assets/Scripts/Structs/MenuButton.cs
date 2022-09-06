using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Murgn
{
    [Serializable]
    public struct MenuButton
    {
        public MenuButtonType menuButtonType;
        public Button button;
        public Image outline;
        public TextMeshProUGUI text;
        public Vector2 textClickedPosition;
    }
}