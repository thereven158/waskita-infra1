using A3.CodePattern;
using UnityEngine;

namespace Agate.WaskitaInfra1.UserInterface.Quiz
{
    public class StringToggleDisplayPool : DataToggleDisplayPool<string>
    {
        [SerializeField]
        private StringToggleDisplay _objectToPool = default;

        protected override DataToggleDisplayBehavior<string> ObjectToPool => _objectToPool;
    }
}