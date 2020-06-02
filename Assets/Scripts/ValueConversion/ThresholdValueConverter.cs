using System;
using System.Collections.Generic;

namespace A3.ValueConversion
{
    public class ThresholdValueConverter<TInput, TOutput> :
        IValueConverter<TInput, TOutput>
        where TInput : IComparable<TInput>
    {
        public ThresholdValueConverter(Dictionary<TInput, TOutput> comparisonDictionary)
        {
            _comparisonDictionary = comparisonDictionary;
        }
        private readonly Dictionary<TInput, TOutput> _comparisonDictionary;

        public TOutput Convert(TInput input)
        {
            TOutput lastValue = default;
            foreach (KeyValuePair<TInput, TOutput> comparisonValue in _comparisonDictionary)
            {
                if (input.CompareTo(comparisonValue.Key) == -1 || input.CompareTo(comparisonValue.Key) == 0)
                    return comparisonValue.Value;
                lastValue = comparisonValue.Value;
            }

            return lastValue;
        }
    }
}