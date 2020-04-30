using System.Collections;
using System.Collections.Generic;
using Agate.WaskitaInfra1.Level;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.BriefDisplay {
    public class BriefScreenController : MonoBehaviour
    {

        [SerializeField]
        private Text _titleProjectText;

        [SerializeField]
        private Image _imageWeather;

        [SerializeField]
        private Image _imageWind;

        [SerializeField]
        private Image _imageSoil;

        [SerializeField]
        private Text _textDuration;

        [SerializeField]
        private Text _textDecription;

        [SerializeField]
        private GameObject _projectCanvas;

        [SerializeField]
        private GameObject _briefCanvas;

        [SerializeField]
        private GameObject _checklistCanvas;

        [SerializeField]
        private Button _btnConfirm;

        [SerializeField]
        private Button _btnCancel;

        private LevelDataScriptableObject leveldata;

        private void Start()
        {
            _btnConfirm.onClick.AddListener(OnConfirm);
            _btnCancel.onClick.AddListener(OnCancel);
            _briefCanvas.SetActive(false);
        }

        public void DisplayBrief(LevelDataScriptableObject data)
        {
            _briefCanvas.SetActive(true);
            _titleProjectText.text = data.name;
            _imageWeather.sprite = data.WeatherForecast.Image;
            _imageWind.sprite = data.SoilCondition.Image;
            _imageSoil.sprite = data.SoilCondition.Image;
            _textDuration.text = data.DayDuration + " Hari";
            _textDecription.text = data.Description;
        }

        public void OnConfirm()
        {
            _briefCanvas.SetActive(false);
            _checklistCanvas.SetActive(true);

        }

        public void OnCancel()
        {
            _projectCanvas.SetActive(true);
        }
    }

}
