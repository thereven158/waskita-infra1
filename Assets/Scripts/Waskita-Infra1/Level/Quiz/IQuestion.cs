using A3.Quiz;

namespace Agate.WaskitaInfra1.Level
{
    public interface IQuestion
    {
        string WrongExplanation { get; }
        string Category { get; }
        IQuiz Quiz { get; }
    }
}