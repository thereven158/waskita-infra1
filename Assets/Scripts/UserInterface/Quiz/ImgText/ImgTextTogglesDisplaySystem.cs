using Agate.WaskitaInfra1.Level;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface.Quiz
{
    public class ImgTextTogglesDisplaySystem : DataTogglesDisplaySystem<ScriptableImgText>
    {
        [SerializeField]
        private ImgTextToggleDisplayPool _togglePool = default;

        [SerializeField]
        private Image _image = default;

        [SerializeField]
        private GameObject _placeholderObject = default;

        protected override DataToggleDisplayPool<ScriptableImgText> TogglePool => _togglePool;

        protected override void OnDataDisplayInteraction(ScriptableImgText obj)
        {
            base.OnDataDisplayInteraction(obj);
            ToggleImageContent(true);
            _image.sprite = obj.Image;
        }

        public override void Reset()
        {
            ToggleImageContent(false);
            base.Reset();
        }

        private void ToggleImageContent(bool toggle)
        {
            _placeholderObject.SetActive(!toggle);
            _image.gameObject.SetActive(toggle);
        }
    }
}