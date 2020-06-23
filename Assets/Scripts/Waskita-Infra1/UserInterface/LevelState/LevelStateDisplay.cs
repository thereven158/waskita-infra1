using A3.ValueConversion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface.LevelState
{
    public class LevelStateDisplay : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _titleText = default;

        [SerializeField]
        private Image _weatherImage = default;

        [SerializeField]
        private TMP_Text _weatherText = default;

        [SerializeField]
        private Image _windImage = default;

        [SerializeField]
        private TMP_Text _windText = default;

        [SerializeField]
        private Image _soilImage = default;

        [SerializeField]
        private TMP_Text _soilText = default;

        [SerializeField]
        private TMP_Text _dayText = default;

        [SerializeField]
        private ScriptableIntToSpriteConverter _windSpriteConverter = default;

        public void OpenDisplay(LevelState levelState)
        {
            gameObject.SetActive(true);
            _titleText.text = levelState.LevelName;
            _weatherImage.sprite = levelState.Weather.Image;
            _weatherText.text = levelState.Weather.name;
            _windImage.sprite = _windSpriteConverter.Convert((int)levelState.WindStrength);
            _windText.text = $"{levelState.WindStrength} km/h";
            _soilImage.sprite = levelState.SoilCondition.Image;
            _soilText.text = $"Tanah {levelState.SoilCondition.name}";
            _dayText.text = $"{levelState.ProjectDuration} Hari";
        }

        public void ToggleDisplay(bool toggle)
        {
            gameObject.SetActive(toggle);
        }
    }
}