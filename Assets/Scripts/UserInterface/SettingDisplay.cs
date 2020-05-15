using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UserInterface
{
    public class SettingDisplay : MonoBehaviour
    {
        [SerializeField]
        private Toggle _bgmToggle;

        [SerializeField]
        private Toggle _sfxToggle;

        [SerializeField]
        private Button _logOutButton;

        [SerializeField]
        private Button _exitButton;

        public UnityAction<bool> OnBgmToggle;
        public UnityAction<bool> OnSfxToggle;
        public UnityAction OnLogOutPress;
        public UnityAction OnExitPress;
        
        public UnityAction OnInteraction;

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
    }
}