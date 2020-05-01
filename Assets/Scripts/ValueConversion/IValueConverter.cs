namespace ValueConversion
{
    public interface IValueConverter<in TInput, out TOutput>
    {
        TOutput Convert(TInput input);
    }
}