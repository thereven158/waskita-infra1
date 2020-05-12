using A3.Quiz;

namespace Agate.WaskitaInfra1.Level
{
    public interface IQuestion
    {
        string WrongExplanation { get; }
        string Category { get; }
        string DisplayName { get; }
        IQuiz Quiz { get; }
    }
}