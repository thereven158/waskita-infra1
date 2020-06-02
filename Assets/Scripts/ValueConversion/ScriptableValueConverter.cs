using UnityEngine;

namespace A3.ValueConversion
{
    [CreateAssetMenu(fileName = "IntToSprite", menuName = "A3/ValueConverter/ThresholdValueConverter", order = 0)]
    public abstract class ScriptableValueConverter<TInput,TOutput> : ScriptableObject, IValueConverter<TInput,TOutput>
    {
        public abstract TOutput Convert(TInput input);
    }
}