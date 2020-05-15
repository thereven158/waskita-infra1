using Agate.WaskitaInfra1.Level;
using UnityEngine;

namespace Agate.WaskitaInfra1.UserInterface.Quiz
{
    public class ImgTextToggleDisplayPool : DataToggleDisplayPool<ScriptableImgText>
    {
        [SerializeField]
        private ImgTextToggleDisplay _objectToPool = default;

        protected override DataToggleDisplayBehavior<ScriptableImgText> ObjectToPool => _objectToPool;
    }
}