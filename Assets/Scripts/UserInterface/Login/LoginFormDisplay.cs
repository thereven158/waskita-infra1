using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface.Login
{
    public class LoginFormDisplay : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField _usernameField = null;

        [SerializeField]
        private TMP_InputField _passwordField = null;

        [SerializeField]
        private Button _loginButton = null;

        public Action ButtonInteractionAction;
        public Action<string, string> LoginAction;

        public void ToggleDisplay(bool toggle)
        {
            gameObject.SetActive(toggle);
        }

        public void Init()
        {
            _loginButton.onClick.AddListener(SubmitLoginForm);
            ToggleDisplay(false);
        }

        private void SubmitLoginForm()
        {
            ButtonInteractionAction?.Invoke();
            LoginAction?.Invoke(_usernameField.text, _passwordField.text);
            _usernameField.text = "";
            _passwordField.text = "";
        }

    }
}