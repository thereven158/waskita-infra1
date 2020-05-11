using System;
using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserInterface.LevelState;

namespace Agate.WaskitaInfra1.UserInterface
{
    public class LevelDataDisplay : MonoBehaviour
    {
        [SerializeField]
        private LevelStateDisplay _stateDisplay;

        [SerializeField]
        private TMP_Text _descriptionText;

        [SerializeField]
        private Button _yesButton;

        [SerializeField]
        private Button _noButton;

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
                $"curah hujan diperikan {levelData.WeatherForecast.RainFall}. " +
                $"Waktu pelaksanaan proyek adalah {levelData.DayDuration}. " +
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