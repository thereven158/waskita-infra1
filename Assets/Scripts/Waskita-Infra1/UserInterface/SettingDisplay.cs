using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface
{
    public class SettingDisplay : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _nikLabel = default;

        [SerializeField]
        private Toggle _bgmToggle = default;

        [SerializeField]
        private Toggle _sfxToggle = default;

        [SerializeField]
        private Button _logOutButton = default;

        [SerializeField]
        private Button _exitButton = default;

        public bool BgmToggleState { get => _bgmToggle.isOn; set => _bgmToggle.isOn = value; }
        public bool SfxToggleState { get => _sfxToggle.isOn; set => _sfxToggle.isOn = value; }
        public string NikText { get => _nikLabel.text.Substring(6); set => _nikLabel.text = $"NIK : {value}"; }

        public UnityAction<bool> OnBgmToggle;
        public UnityAction<bool> OnSfxToggle;
        public UnityAction OnLogOutPress;
        public UnityAction OnExitPress;
        public UnityAction OnInteraction;
        public UnityAction OnClose;

        private void Awake()
        {
            _bgmToggle.onValueChanged.AddListener(toggle => OnInteraction?.Invoke());
            _sfxToggle.onValueChanged.AddListener(toggle => OnInteraction?.Invoke());
            _logOutButton.onClick.AddListener(() => OnInteraction?.Invoke());
            _exitButton.onClick.AddListener(() => OnInteraction?.Invoke());

            _bgmToggle.onValueChanged.AddListener(toggle => OnBgmToggle?.Invoke(toggle));
            _sfxToggle.onValueChanged.AddListener(toggle => OnSfxToggle?.Invoke(toggle));
            _logOutButton.onClick.AddListener(() => OnLogOutPress?.Invoke());
            _exitButton.onClick.AddListener(() => OnExitPress?.Invoke());
        }

        public void ToggleDisplay(bool toggle)
        {
            if (gameObject.activeSelf == toggle) return;
            OnInteraction?.Invoke();
            gameObject.SetActive(toggle);
            if (toggle) return;
            OnClose?.Invoke();
        }
    }
}