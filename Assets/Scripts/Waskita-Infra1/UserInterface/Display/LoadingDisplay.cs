using A3.UserInterface;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface
{
    public class LoadingDisplay : DisplayBehavior
    {
        [SerializeField]
        private Slider _progressSlider = default;
        public override bool IsOpen => gameObject.activeSelf;

        public void Open()
        {
            gameObject.SetActive(true);
            _progressSlider.value = 0;
        }

        public void UpdateDisplay(float progress)
        {
            _progressSlider.value = progress;
        }

        public override void Close()
        {
            gameObject.SetActive(false);
        }

        public override void Init()
        {
            gameObject.SetActive(false);
            _progressSlider.value = 0;
        }
    }
}