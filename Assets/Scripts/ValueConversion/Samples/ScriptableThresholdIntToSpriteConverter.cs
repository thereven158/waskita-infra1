using System;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace ValueConversion
{
    [CreateAssetMenu(fileName = "ThreshIntToSpr", menuName = "A3/ValuC/Thr/IntToSp")]
    public class ScriptableThresholdIntToSpriteConverter : ScriptableIntToSpriteConverter
    {
        [SerializeField]
        private ThresholdDictionary _comparisonDictionary = default;

        private ThresholdValueConverter<int, Sprite> _converter;

        private void Awake()
        {
            _converter = _converter ?? new ThresholdValueConverter<int, Sprite>(_comparisonDictionary.Clone());
        }
        private void OnEnable()
        {
            _converter = _converter ?? new ThresholdValueConverter<int, Sprite>(_comparisonDictionary.Clone());
        }

        public override Sprite Convert(int input)
        {
            return _converter.Convert(input);
        }
    }

    [Serializable]
    public class ThresholdDictionary : SerializableDictionaryBase<int, Sprite>
    {
    }
}