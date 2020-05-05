using UnityEngine;

namespace ValueConversion
{
    public abstract class ScriptableIntToSpriteConverter: ScriptableValueConverter<int,Sprite>
    {
        public abstract override Sprite Convert(int input);
    }
}