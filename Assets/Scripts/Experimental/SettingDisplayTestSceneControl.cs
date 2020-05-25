using Agate.WaskitaInfra1.UserInterface;
using UnityEngine;

namespace Experimental
{
    public class SettingDisplayTestSceneControl : MonoBehaviour
    {
        [SerializeField]
        private SettingDisplay _settingDisplay = default;

        private void Start()
        {
            _settingDisplay.OnInteraction = () => Debug.Log("Interaction");
        }
    }
}