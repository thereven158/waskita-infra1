using A3.Quiz;
using Agate.WaskitaInfra1.Animations;

namespace Agate.WaskitaInfra1.Level
{
    public interface IQuestion
    {
        string Category { get; }
        string DisplayName { get; }
        IQuiz Quiz { get; }
        string WrongExplanation { get; }
        AnimationSceneControl WrongAnimation { get; }
    }
}