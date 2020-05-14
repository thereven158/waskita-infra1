using UnityEngine;
using UnityEngine.UI;

namespace Agate.GlSim.Util
{
    public class SliderToggle : MonoBehaviour
    {
        [SerializeField]
        private Slider slider = null;

        public void Toggle(bool toggle)
        {
            slider.value = toggle ? 1 : 0;
        }
    }
}