using Agate.WaskitaInfra1.PlayerAccount;
using Agate.WaskitaInfra1.UserInterface.Login;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.SceneControl.Login
{
    public class TitleSceneControl : MonoBehaviour
    {
       
        [SerializeField]
        private LoginFormDisplay _loginForm = null;

        [SerializeField]
        private Button _startButton = null;

        [SerializeField]
        private Button _logOutButton = null;

        private bool loggedIn;

        private void Start()
        {
            loggedIn = !Main.GetRegisteredComponent<PlayerAccountControl>().Data.IsEmpty();
            _loginForm.Init();
            _loginForm.LoginAction = OnLoginButton;
            _logOutButton.gameObject.SetActive(loggedIn);
            _logOutButton.onClick.AddListener(OnLogoutButton);
            _startButton.onClick.AddListener(OnStartButtonPress);
            
        }

        private void OnStartButtonPress()
        {
            // if (loggedIn) Main.Instance.StartGame();
            _loginForm.ToggleDisplay(true);
        }

        private void OnLoginButton(string username, string password)
        {
            // if (Main.Instance.OfflineMode) Main.Instance.StartGame();
            // else StartCoroutine(_backendIntegration.LoginProcess(username, password, Main.Instance.StartGame, null));
            Main.Instance.StartGame();
        }
        private void OnLogoutButton()
        {
            Main.Quit();
            // if (Main.Instance.OfflineMode) Main.Instance.StartGame();
            // else StartCoroutine(_backendIntegration.LoginProcess(username, password, Main.Instance.StartGame, null));
        }
    }
}