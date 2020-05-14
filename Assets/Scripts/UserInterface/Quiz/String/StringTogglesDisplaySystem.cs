using UnityEngine;

namespace Agate.WaskitaInfra1.UserInterface.Quiz
{
    public class StringTogglesDisplaySystem : DataTogglesDisplaySystem<string>
    {
        [SerializeField]
        private StringToggleDisplayPool _togglePool;
        
        protected override DataToggleDisplayPool<string> TogglePool => _togglePool;
    }
}