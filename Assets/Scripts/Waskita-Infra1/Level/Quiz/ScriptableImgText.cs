using UnityEngine;

namespace Agate.WaskitaInfra1.Level
{
    public class ScriptableImgText : ScriptableObject
    {
        public Sprite Image;

        [TextArea]
        public string Text;
    }
}