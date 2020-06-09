using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.UserInterface.LevelState;
using Agate.WaskitaInfra1.Utilities;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface
{
    public class LevelDataDisplay : MonoBehaviour
    {
        [SerializeField]
        private LevelStateDisplay _stateDisplay = default;

        [SerializeField]
        private TMP_Text _descriptionText = default;

        [SerializeField]
        private Button _yesButton = default;

        [SerializeField]
        private Button _noButton = default;

        private Action<LevelData> _onYes;
        private Action _onNo;

        private LevelData data;

        private void Awake()
        {
            _yesButton.onClick.AddListener(YesAction);
            _noButton.onClick.AddListener(NoAction);
        }

        public void OpenDisplay(LevelData levelData, Action<LevelData> onYes, Action onNo)
        {
            data = levelData;
            _stateDisplay.OpenDisplay(levelData.State());
            _descriptionText.text =
                $"Proyek kali ini akan dilakukan di {levelData.Location}. " +
                $"Sekarang sedang musim {levelData.WeatherForecast.Season}, " +
                $"curah hujan diperkirakan {levelData.WeatherForecast.RainFall}. " +
                $"Waktu pelaksanaan proyek adalah {levelData.DayDuration} hari. " +
                $"Selamat bertugas dan semoga sukses!";
            _onYes = onYes;
            _onNo = onNo;
            ToggleDisplay(true);
        }

        public void ToggleDisplay(bool toggle)
        {
            gameObject.SetActive(toggle);
        }

        private void YesAction()
        {
            _onYes?.Invoke(data);
            ToggleDisplay(false);
        }

        private void NoAction()
        {
            _onNo?.Invoke();
            ToggleDisplay(false);
        }
    }
}