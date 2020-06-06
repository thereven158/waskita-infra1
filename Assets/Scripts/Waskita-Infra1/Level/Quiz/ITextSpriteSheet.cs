using Agate.SpriteSheet;

namespace Agate.WaskitaInfra1.Level
{
    public interface ITextSpriteSheet
    {
        string Text { get; }
        ISpriteSheet SpriteSheet { get; }
    }
}